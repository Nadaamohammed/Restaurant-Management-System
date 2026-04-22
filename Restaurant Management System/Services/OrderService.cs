using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System;
using Restaurant_Management_System.Enum;
using Restaurant_Management_System.Models;

namespace RestaurantManagementSystem.Services;

public class OrderService
{
    private readonly ResaturantContext _context;
    private readonly InventoryService _inventoryService;

    public OrderService(ResaturantContext context, InventoryService inventoryService)
    {
        _context = context;
        _inventoryService = inventoryService;
    }

    // ── Place Order ─────────────────────────────────────────────────────
    // Returns: (Success, Message, OrderId) --> tuple with success flag, message, and created order ID (0 if failed)
    public (bool Success, string Message, int OrderId) PlaceOrder(
        int employeeId,
        int customerId,
        int branchId,
        OrderType orderType,
        string? deliveryAddress,
        // Each OrderItem: (ItemId, Quantity, SpecialNotes, List of AddOnIds)
        List<(int ItemId, int Qty, string? Notes, List<int>? AddOnIds)> items)
    {
        using var transaction = _context.Database.BeginTransaction();

        try
        {
            // only waiters and cashiers can place orders, and must be assigned to the branch
            var employee = _context.Employees
                .FirstOrDefault(e => e.EmployeeId == employeeId);
            // test waiter/cashier exists, is correct role, and assigned to branch

            if (employee is null)
                return (false, "Employee not found.", 0);

            if (employee is not Waiter && employee is not Cashier)
                return (false, "Only Waiters and Cashiers can place orders.", 0);

            if (!employee.AssignedBranchIds.Contains(branchId))
                return (false, "Employee not assigned to branch.", 0);

            var customer = _context.Customers.Find(customerId);
            if (customer is null)
                return (false, "Customer not found.", 0);

            var branchExists = _context.Branches.Any(b => b.BranchId == branchId);
            if (!branchExists)
                return (false, "Branch not found.", 0);

            if (orderType == OrderType.Delivery && string.IsNullOrWhiteSpace(deliveryAddress))
                return (false, "Delivery requires address.", 0);

            if (items == null || items.Count == 0)
                return (false, "Order must contain items.", 0);

            var orderItems = new List<OrderItem>();
            decimal total = 0;

            foreach (var (itemId, qty, notes, addOnIds) in items)
            {
                var branchItem = _context.BranchMenuItems
                    .FirstOrDefault(b => b.BranchId == branchId && b.ItemId == itemId);

                if (branchItem == null || !branchItem.IsAvailable)
                    return (false, $"Item {itemId} unavailable.", 0);

                var menuItem = _context.MenuItems
                    .Include(m => m.AddOns)
                    .FirstOrDefault(m => m.ItemId == itemId);

                if (menuItem == null)
                    return (false, $"Item {itemId} not found.", 0);

                decimal price = branchItem.PriceOverride ?? menuItem.BasePrice;

                var safeAddOns = addOnIds ?? new List<int>();

                foreach (var addOnId in safeAddOns)
                {
                    var addOn = menuItem.AddOns.FirstOrDefault(a => a.AddOnId == addOnId);
                    if (addOn == null)
                        return (false, $"Invalid add-on.", 0);

                    price += addOn.ExtraPrice;
                }

                orderItems.Add(new OrderItem
                {
                    OrderItemId = itemId,
                    Quantity = qty,
                    UnitPrice = price,
                    SpecialNotes = notes,
                    SelectedAddOnIds = safeAddOns
                });

                total += price * qty;
            }

            var order = new Order
            {
                Type = orderType,
                OrderDateTime = DateTime.Now,
                TotalAmount = total,
                Status = OrderStatus.Pending,
                BranchId = branchId,
                HandledByEmployeeId = employeeId,
                CustomerId = customerId,
                DeliveryAddress = deliveryAddress,
                OrderItems = orderItems
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            // Delivery creation
            if (orderType == OrderType.Delivery)
            {
                _context.Deliveries.Add(new Delivery
                {
                    OrderId = order.OrderId,
                    DeliveryAddress = deliveryAddress!,
                    Status = DeliveryStatus.AwaitingAssignment
                });

                _context.SaveChanges();
            }

            transaction.Commit();

            return (true, "Order placed.", order.OrderId);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    // ── Start Preparing ─────────────────────────────────────────────────
    public (bool Success, string Message) StartPreparing(
        int orderId, int chefId, int? managerOverrideId = null)
    {
        var order = _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefault(o => o.OrderId == orderId);

        if (order is null)
            return (false, "Order not found.");

        if (order.Status != OrderStatus.Pending)
            return (false, "Order must be Pending.");

        var chef = _context.Employees.Find(chefId);
        if (chef is not Chef)
            return (false, "Only chefs allowed.");

        if (!chef.AssignedBranchIds.Contains(order.BranchId))
            return (false, "Chef not assigned.");

        var sufficient = _inventoryService.IsSufficient(order.BranchId, order.OrderItems);

        if (!sufficient)
        {
            if (managerOverrideId == null)
                return (false, "INSUFFICIENT_STOCK");

            var manager = _context.Employees.Find(managerOverrideId);
            if (manager is not BranchManager ||
                !manager.AssignedBranchIds.Contains(order.BranchId))
                return (false, "Override denied.");
        }

        _inventoryService.Deduct(order.BranchId, order.OrderItems);

        order.Status = OrderStatus.Preparing;
        _context.SaveChanges();

        return (true, "Preparing started.");
    }

    // ── Serve Order ─────────────────────────────────────────────────────
    public (bool Success, string Message) ServeOrder(int orderId, int chefId)
    {
        var order = _context.Orders.Find(orderId);
        if (order is null) return (false, "Not found.");
        if (order.Status != OrderStatus.Preparing)
            return (false, "Must be Preparing.");

        var chef = _context.Employees.Find(chefId);
        if (chef is not Chef)
            return (false, "Only chefs.");

        order.Status = OrderStatus.Served;
        _context.SaveChanges();

        return (true, "Served.");
    }

    // ── Process Payment ─────────────────────────────────────────────────
    public (bool Success, string Message) ProcessPayment(
        int orderId, int cashierId, PaymentMethod method)
    {
        var order = _context.Orders.Find(orderId);
        if (order is null) return (false, "Not found.");
        if (order.Status != OrderStatus.Served)
            return (false, "Must be Served.");

        var cashier = _context.Employees.Find(cashierId);
        if (cashier is not Cashier)
            return (false, "Only cashier.");

        order.PaymentMethod = method;

        if (order.Type == OrderType.DineIn)
        {
            order.Status = OrderStatus.Completed;

            var customer = _context.Customers.Find(order.CustomerId);
            if (customer != null)
                customer.LoyaltyPoints += (int)Math.Floor(order.TotalAmount);
        }

        _context.SaveChanges();

        return (true, "Payment processed.");
    }

    // ── Cancel Order ────────────────────────────────────────────────────
    public (bool Success, string Message) CancelOrder(int orderId, int employeeId)
    {
        var order = _context.Orders.Find(orderId);
        if (order is null) return (false, "Not found.");

        if (order.Status == OrderStatus.Completed || order.Status == OrderStatus.Cancelled)
            return (false, "Cannot cancel.");

        var emp = _context.Employees.Find(employeeId);
        if (emp == null) return (false, "Employee not found.");

        if (!emp.AssignedBranchIds.Contains(order.BranchId))
            return (false, "Wrong branch.");

        if (emp is Waiter or Cashier)
        {
            if (order.Status != OrderStatus.Pending)
                return (false, "Only Pending.");
        }
        else if (emp is BranchManager)
        {
            var delivery = _context.Deliveries
                .FirstOrDefault(d => d.OrderId == orderId);

            if (delivery?.Status == DeliveryStatus.OnTheWay)
                return (false, "Already out.");
        }
        else
            return (false, "Unauthorized.");

        order.Status = OrderStatus.Cancelled;
        _context.SaveChanges();

        return (true, "Canceled.");
    }
}