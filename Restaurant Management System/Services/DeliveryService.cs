using Restaurant_Management_System;
using Restaurant_Management_System.Enum;
using Restaurant_Management_System.Models;

namespace RestaurantManagementSystem.Services;

public class DeliveryService
{
    private readonly ResaturantContext _context;

    public DeliveryService(ResaturantContext context)
    {
        _context = context;
    }

    // ── Assign Delivery Staff ─────────────────────────────────────────────
    public (bool Success, string Message) AssignStaff(
        int orderId, int deliveryStaffId, int assignedByEmployeeId)
    {
        var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
        if (order is null)
            return (false, "Order not found.");

        if (order.Type != OrderType.Delivery)
            return (false, "This is not a delivery order.");

        if (order.Status != OrderStatus.Served)
            return (false, "Only Served orders can be assigned.");

        var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == assignedByEmployeeId);
        if (employee is null)
            return (false, "Employee not found.");

        if (employee is not Waiter && employee is not Cashier && employee is not BranchManager)
            return (false, "Unauthorized role.");

        if (!employee.AssignedBranchIds.Contains(order.BranchId))
            return (false, "Employee not assigned to this branch.");

        var staff = _context.DeliversesStaff.FirstOrDefault(s => s.StaffId == deliveryStaffId);
        if (staff is null)
            return (false, "Delivery staff not found.");

        if (!staff.IsAvailable)
            return (false, "Delivery staff unavailable.");

        if (staff.BranchId != order.BranchId)
            return (false, "Staff must belong to same branch.");

        var delivery = _context.Deliveries.FirstOrDefault(d => d.OrderId == orderId);
        if (delivery is null)
            return (false, "Delivery record not found.");

        if (delivery.DeliveryStaffId != null)
            return (false, "Delivery already assigned.");

        delivery.DeliveryStaffId = deliveryStaffId;
        delivery.Status = DeliveryStatus.OnTheWay;
        staff.IsAvailable = false;

        _context.SaveChanges();

        return (true, $"Assigned to {staff.FullName}. Status: On The Way.");
    }

    // ── Mark Delivered ────────────────────────────────────────────────────
    public (bool Success, string Message) MarkDelivered(int deliveryId, int deliveryStaffId)
    {
        var delivery = _context.Deliveries.FirstOrDefault(d => d.DeliveryId == deliveryId);
        if (delivery is null)
            return (false, "Delivery not found.");

        if (delivery.Status != DeliveryStatus.OnTheWay)
            return (false, "Delivery is not in transit.");

        if (delivery.DeliveryStaffId != deliveryStaffId)
            return (false, "Unauthorized staff.");

        delivery.Status = DeliveryStatus.Delivered;
        delivery.DeliveryTime = DateTime.Now;

        var order = _context.Orders.FirstOrDefault(o => o.OrderId == delivery.OrderId);
        if (order != null)
        {
            order.Status = OrderStatus.Completed;

            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == order.CustomerId);
            if (customer != null)
                customer.LoyaltyPoints += (int)Math.Floor(order.TotalAmount);
        }

        var staff = _context.DeliversesStaff.FirstOrDefault(s => s.StaffId == deliveryStaffId);
        if (staff != null)
            staff.IsAvailable = true;

        _context.SaveChanges();

        return (true, "Delivery completed and order closed.");
    }

    // ── Mark Failed ───────────────────────────────────────────────────────
    public (bool Success, string Message) MarkFailed(
        int deliveryId, int deliveryStaffId, string reason)
    {
        var delivery = _context.Deliveries.FirstOrDefault(d => d.DeliveryId == deliveryId);
        if (delivery is null)
            return (false, "Delivery not found.");

        if (delivery.Status != DeliveryStatus.OnTheWay)
            return (false, "Delivery is not in transit.");

        if (delivery.DeliveryStaffId != deliveryStaffId)
            return (false, "Unauthorized staff.");

        if (string.IsNullOrWhiteSpace(reason))
            return (false, "Failure reason required.");

        delivery.Status = DeliveryStatus.Failed;
        delivery.FailureReason = reason;
        delivery.DeliveryTime = DateTime.Now;

        var order = _context.Orders.FirstOrDefault(o => o.OrderId == delivery.OrderId);
        if (order != null)
            order.Status = OrderStatus.Cancelled;

        var staff = _context.DeliversesStaff.FirstOrDefault(s => s.StaffId == deliveryStaffId);
        if (staff != null)
            staff.IsAvailable = true;

        _context.SaveChanges();

        return (true, "Delivery failed and order canceled.");
    }
}