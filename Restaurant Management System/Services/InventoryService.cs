using Restaurant_Management_System;
using Restaurant_Management_System.Models;

namespace RestaurantManagementSystem.Services;

public class InventoryService
{
    private readonly ResaturantContext _context;

    public InventoryService(ResaturantContext context)
    {
        _context = context;
    }
    // returns true if the branch has enough inventory to fulfill the order items, false otherwise
    public bool IsSufficient(int branchId, List<OrderItem> items)
        => GetShortfalls(branchId, items).Count == 0;

    public Dictionary<int, double> GetShortfalls(int branchId, List<OrderItem> items)
    {
        var required = AggregateRequired(items);
        // shortfalls dictionary to track ingredient deficits (عجز المكونات)
        var shortfalls = new Dictionary<int, double>();

        foreach (var (ingredientId, qty) in required)
        {
            var inventory = _context.BranchInventories
                .FirstOrDefault(i => i.BranchId == branchId && i.IngredientId == ingredientId);

            double available = inventory?.CurrentQuantity ?? 0;
            // ✅ correct shortfall calculation
            // اكيد انو اذا الكمية المتوفرة اقل من المطلوبة نضيف الفرق الى العجز

            if (available < qty)
                shortfalls[ingredientId] = qty - available;
        }

        return shortfalls;
    }
    //مجموع الحاجات المطلوبة لكل مكون بناءً على الطلبات

    public void Deduct(int branchId, List<OrderItem> items)
    {
        var required = AggregateRequired(items);

        foreach (var (ingredientId, qty) in required)
        {
            var inventory = _context.BranchInventories
                .FirstOrDefault(i => i.BranchId == branchId && i.IngredientId == ingredientId);

            if (inventory == null || inventory.CurrentQuantity < qty)
                throw new Exception($"Not enough stock for ingredient {ingredientId}");

            // ✅ correct deduction

            inventory.CurrentQuantity -= qty;
        }

        _context.SaveChanges();
    }
    // needed ingredients aggregation based on order items and their recipes

    private Dictionary<int, double> AggregateRequired(List<OrderItem> items)
    {
        var required = new Dictionary<int, double>();

        foreach (var item in items)
        {
            var recipes = _context.Recipes
                .Where(r => r.MenuItemId == item.OrderItemId)
                .ToList();

            foreach (var recipe in recipes)
            {
                if (!required.ContainsKey(recipe.IngredientId))
                    required[recipe.IngredientId] = 0;

                required[recipe.IngredientId] +=
                    (double)recipe.QuantityRequired * item.Quantity;
            }
        }

        return required;
    }
}