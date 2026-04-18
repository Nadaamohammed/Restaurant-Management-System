namespace Restaurant_Management_System.Seeds
{
    using Restaurant_Management_System.Models;
    using System.Linq;
    public class DataSeeds
    {
        public static void Seed(ResaturantContext context)
        {
            // Seed Branches
            if (!context.Branches.Any())
            {
                context.Branches.AddRange(new[]
    {
            new Branches { BranchId = 1, BranchName = "Downtown Branch", BranchAddress = "123 Main St, Cairo",ContactNumber = "02-555-0101", OpeningHours = "08:00–23:00", ManagerId = 5 },
            new Branches { BranchId = 2, BranchName = "Mall Branch",BranchAddress = "456 Mall Rd, Giza",ContactNumber = "02-555-0202", OpeningHours = "10:00–22:00", ManagerId = 6 }
        });
                context.SaveChanges();
            }

            // Seed Employees
            if (!context.Employees.Any())
            {
                var downtownBranch = context.Branches.FirstOrDefault(b => b.BranchName == "Downtown");
                var uptownBranch = context.Branches.FirstOrDefault(b => b.BranchName == "Uptown");

                context.Employees.AddRange(new Employee[]
        {
            new Chef         { EmployeeId = 1, FullName = "Ali Hassan",      Salary = 5000, DateOfHire = new DateTime(2020,  1, 15), ContactInfo = "ali@restaurant.com",    BranchId = 1, AssignedBranchIds = new() { 1 } },
            new Waiter       { EmployeeId = 2, FullName = "Sara Ahmed",      Salary = 3000, DateOfHire = new DateTime(2021,  3, 10), ContactInfo = "sara@restaurant.com",   BranchId = 1, AssignedBranchIds = new() { 1 } },
            new Cashier      { EmployeeId = 3, FullName = "Mohamed Khaled",  Salary = 3500, DateOfHire = new DateTime(2021,  5, 20), ContactInfo = "mo@restaurant.com",     BranchId = 1, AssignedBranchIds = new() { 1 } },
            new Chef         { EmployeeId = 4, FullName = "Nour Ibrahim",    Salary = 5500, DateOfHire = new DateTime(2019,  8,  5), ContactInfo = "nour@restaurant.com",   BranchId = 2, AssignedBranchIds = new() { 2 } },
            new BranchManager{ EmployeeId = 5, FullName = "Karim Youssef",   Salary = 8000, DateOfHire = new DateTime(2018,  2,  1), ContactInfo = "karim@restaurant.com",  BranchId = 1, AssignedBranchIds = new() { 1 } },
            new BranchManager{ EmployeeId = 6, FullName = "Dina Samir",      Salary = 8500, DateOfHire = new DateTime(2018,  6, 15), ContactInfo = "dina@restaurant.com",   BranchId = 2, AssignedBranchIds = new() { 2 } },
            new Waiter       { EmployeeId = 7, FullName = "Omar Fares",      Salary = 3000, DateOfHire = new DateTime(2022,  1,  1), ContactInfo = "omar@restaurant.com",   BranchId = 2, AssignedBranchIds = new() { 2 } },
            new Cashier      { EmployeeId = 8, FullName = "Layla Nabil",     Salary = 3500, DateOfHire = new DateTime(2022,  4, 10), ContactInfo = "layla@restaurant.com",  BranchId = 2, AssignedBranchIds = new() { 2 } },
            new Chef         { EmployeeId = 9, FullName = "Reem Fathy",      Salary = 5200, DateOfHire = new DateTime(2023,  2,  1), ContactInfo = "reem@restaurant.com",   BranchId = 1, AssignedBranchIds = new() { 1, 2 } }
        });
                context.SaveChanges();

                // ─── Delivery Staff ─────────────────────────────────────────────────────4
                if (!context.DeliversesStaff.Any())
                {
                    context.DeliversesStaff.AddRange(new[]
                {
            new DeliveryStaff { StaffId = 1, FullName = "Tarek Mansour", VehicleType = "Motorcycle", LicenseNumber = "LIC-001", AssignedArea = "Downtown",     BranchId = 1, IsAvailable = true },
            new DeliveryStaff { StaffId = 2, FullName = "Rania Hassan",  VehicleType = "Bicycle",    LicenseNumber = "LIC-002", AssignedArea = "Mall District", BranchId = 2, IsAvailable = true }
        });
                }
                context.SaveChanges();



                // ─── Customers ──────────────────────────────────────────────────────────
                if (!context.Customers.Any())
                {
                    context.Customers.AddRange(new[]
                {
            new Customer { CustomerId = 1, FullName = "Ahmed Sayed",  PhoneNumber = "010-1111111", Email = "ahmed@email.com", LoyaltyPoints = 150 },
            new Customer { CustomerId = 2, FullName = "Mona Adel",    PhoneNumber = "010-2222222", Email = "mona@email.com",  LoyaltyPoints = 80  },
            new Customer { CustomerId = 3, FullName = "Hassan Ali",   PhoneNumber = "010-3333333", Email = "hassan@email.com",LoyaltyPoints = 0   }
        });

                }
                context.SaveChanges();



                // ─── Add-Ons ─────────────────────────────────────────────────────────────
                if (!context.AddOns.Any())
                {
                    var extraCheese = new AddOn { AddOnId = 1, Name = "Extra Cheese", ExtraPrice = 5 };
                    var largeSize = new AddOn { AddOnId = 2, Name = "Large Size", ExtraPrice = 10 };
                    var extraSauce = new AddOn { AddOnId = 3, Name = "Extra Sauce", ExtraPrice = 3 };
                    {


                        // ─── Menu Items ──────────────────────────────────────────────────────────
                        if (context.MenuItems.Any())
                        {
                            context.MenuItems.AddRange(new[]
               {
            new MenuItem { ItemId = 1, Name = "Spring Rolls",    Description = "Crispy veggie rolls",        BasePrice = 25, Category = "Appetizer",    AddOns = new() { extraSauce } },
            new MenuItem { ItemId = 2, Name = "Grilled Chicken", Description = "Charcoal grilled chicken",   BasePrice = 75, Category = "Main Course",  AddOns = new() { extraCheese, largeSize } },
            new MenuItem { ItemId = 3, Name = "Chocolate Cake",  Description = "Rich chocolate dessert",     BasePrice = 40, Category = "Dessert" },
            new MenuItem { ItemId = 4, Name = "Fresh Juice",     Description = "Seasonal fruit juice",       BasePrice = 20, Category = "Beverage",     AddOns = new() { largeSize } },
            new MenuItem { ItemId = 5, Name = "Caesar Salad",    Description = "Classic caesar salad",       BasePrice = 35, Category = "Appetizer" },
            new MenuItem { ItemId = 6, Name = "Beef Burger",     Description = "Juicy beef patty burger",    BasePrice = 65, Category = "Main Course",  AddOns = new() { extraCheese, extraSauce } }
        });

                        }
                        context.SaveChanges();


                        // ─── Branch Menu Items ───────────────────────────────────────────────────
                        if (!context.BranchMenuItems.Any())
                        {
                            context.BranchMenuItems.AddRange(new[]
                            {
                            // Branch 1 — Caesar Salad unavailable; Grilled Chicken has price override
                            new BranchMenuItem { BranchId = 1, ItemId = 1, IsAvailable = true },
            new BranchMenuItem { BranchId = 1, ItemId = 2, IsAvailable = true, PriceOverride = 80 },
            new BranchMenuItem { BranchId = 1, ItemId = 3, IsAvailable = true },
            new BranchMenuItem { BranchId = 1, ItemId = 4, IsAvailable = true },
            new BranchMenuItem { BranchId = 1, ItemId = 5, IsAvailable = false },
            new BranchMenuItem { BranchId = 1, ItemId = 6, IsAvailable = true },
            // Branch 2 — Beef Burger unavailable; Chocolate Cake has price override
            new BranchMenuItem { BranchId = 2, ItemId = 1, IsAvailable = true },
            new BranchMenuItem { BranchId = 2, ItemId = 2, IsAvailable = true },
            new BranchMenuItem { BranchId = 2, ItemId = 3, IsAvailable = true, PriceOverride = 45 },
            new BranchMenuItem { BranchId = 2, ItemId = 4, IsAvailable = true },
            new BranchMenuItem { BranchId = 2, ItemId = 5, IsAvailable = true },
            new BranchMenuItem { BranchId = 2, ItemId = 6, IsAvailable = false }

                            });
                        }
                        context.SaveChanges();


                        // ─── Ingredients ─────────────────────────────────────────────────────────

                        if (!context.Ingredients.Any())
                        {
                            context.Ingredients.AddRange(new[]
                        {
            new Ingredient { IngredientId = 1, Name = "Chicken",      Unit = "kg"     },
            new Ingredient { IngredientId = 2, Name = "Flour",        Unit = "kg"     },
            new Ingredient { IngredientId = 3, Name = "Tomato Sauce", Unit = "liter"  },
            new Ingredient { IngredientId = 4, Name = "Cheese",       Unit = "kg"     },
            new Ingredient { IngredientId = 5, Name = "Beef Patty",   Unit = "piece"  },
            new Ingredient { IngredientId = 6, Name = "Lettuce",      Unit = "kg"     },
            new Ingredient { IngredientId = 7, Name = "Chocolate",    Unit = "kg"     }
        });
                        }
                        context.SaveChanges();


                        // ─── Branch Inventory ─────────────────────────────────────────────────────
                        if (context.Ingredients.Any())
                            {


                                context.BranchInventories.AddRange(new[]
                                {
            new BranchInventory { BranchId = 1, IngredientId = 1, CurrentQuantity = 20 },
            new BranchInventory { BranchId = 1, IngredientId = 2, CurrentQuantity = 15 },
            new BranchInventory { BranchId = 1, IngredientId = 3, CurrentQuantity = 10 },
            new BranchInventory { BranchId = 1, IngredientId = 4, CurrentQuantity =  8 },
            new BranchInventory { BranchId = 1, IngredientId = 5, CurrentQuantity = 30 },
            new BranchInventory { BranchId = 1, IngredientId = 6, CurrentQuantity =  5 },
            new BranchInventory { BranchId = 1, IngredientId = 7, CurrentQuantity =  6 },
            new BranchInventory { BranchId = 2, IngredientId = 1, CurrentQuantity = 25 },
            new BranchInventory { BranchId = 2, IngredientId = 2, CurrentQuantity = 20 },
            new BranchInventory { BranchId = 2, IngredientId = 3, CurrentQuantity = 12 },
            new BranchInventory { BranchId = 2, IngredientId = 4, CurrentQuantity = 10 },
            new BranchInventory { BranchId = 2, IngredientId = 5, CurrentQuantity = 25 },
            new BranchInventory { BranchId = 2, IngredientId = 6, CurrentQuantity =  8 },
            new BranchInventory { BranchId = 2, IngredientId = 7, CurrentQuantity =  4 }
        });
                            }
                        context.SaveChanges();


                        // ─── Recipes ─────────────────────────────────────────────────────────────
                        // Spring Rolls (1): Flour + Sauce + Lettuce
                        if (!context.Recipes.Any())
                        {
                            context.Recipes.AddRange(new[]
                            {
        new Recipe { MenuItemId = 1, IngredientId = 2, QuantityRequired = 0.10m },
        new Recipe { MenuItemId = 1, IngredientId = 3, QuantityRequired = 0.05m },
        new Recipe { MenuItemId = 1, IngredientId = 6, QuantityRequired = 0.05m },

        new Recipe { MenuItemId = 2, IngredientId = 1, QuantityRequired = 0.30m },

        new Recipe { MenuItemId = 3, IngredientId = 2, QuantityRequired = 0.20m },
        new Recipe { MenuItemId = 3, IngredientId = 7, QuantityRequired = 0.10m },

        new Recipe { MenuItemId = 5, IngredientId = 6, QuantityRequired = 0.15m },
        new Recipe { MenuItemId = 5, IngredientId = 4, QuantityRequired = 0.05m },

        new Recipe { MenuItemId = 6, IngredientId = 5, QuantityRequired = 1.00m },
        new Recipe { MenuItemId = 6, IngredientId = 6, QuantityRequired = 0.05m },
        new Recipe { MenuItemId = 6, IngredientId = 3, QuantityRequired = 0.03m }
    });
                        }
                            context.SaveChanges();

                            // ─── Shift Schedules (sample) ─────────────────────────────────────────────
                            if (!context.ShiftSchedules.Any())
                        {
                            context.ShiftSchedules.AddRange(new[]
                        {
            new ShiftSchedule { ScheduleId = 1, BranchId = 1, EmployeeId = 1, Date = DateTime.Today, TimeSlot = "08:00–16:00" },
            new ShiftSchedule { ScheduleId = 2, BranchId = 1, EmployeeId = 2, Date = DateTime.Today, TimeSlot = "10:00–18:00" },
            new ShiftSchedule { ScheduleId = 3, BranchId = 1, EmployeeId = 3, Date = DateTime.Today, TimeSlot = "12:00–20:00" }
        });
                        }
                        context.SaveChanges();

                    }
                }
            }
        }
    }
}