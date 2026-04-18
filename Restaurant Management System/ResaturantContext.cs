using Microsoft.EntityFrameworkCore;
using Restaurant_Management_System.Models;

namespace Restaurant_Management_System
{
    public class ResaturantContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(
                "Server=.;Database=RestaurantDB;Trusted_Connection=True;TrustServerCertificate=True");
        }
        public DbSet<Branches> Branches { get; set; }
        public DbSet<BranchInventory> BranchInventories { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShiftSchedule> ShiftSchedules { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Waiter> Waiters { get; set; }
        public DbSet<Chef> Chefs { get; set; }
        public DbSet<BranchManager> Managers { get; set; }
        public DbSet<AddOn> AddOns { get; set; }
        public DbSet<BranchMenuItem> BranchMenuItems { get; set; }
        public DbSet<Cashier> Cashier { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }

        public DbSet<DeliveryStaff> DeliversesStaff { get; set; }
        public DbSet<Recipe> Recipes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Composite Keys
            modelBuilder.Entity<Recipe>()
                .HasKey(r => new { r.MenuItemId, r.IngredientId });

            modelBuilder.Entity<Branches>()
        .HasKey(b => b.BranchId);

            modelBuilder.Entity<DeliveryStaff>().HasKey(ds => ds.StaffId);
            modelBuilder.Entity<MenuItem>().HasKey(mi => mi.ItemId);
            modelBuilder.Entity<Order>().HasKey(o => o.OrderId);
            modelBuilder.Entity<OrderItem>().HasKey(oi => oi.OrderItemId);
            modelBuilder.Entity<ShiftSchedule>().HasKey(ss => ss.ScheduleId);
            modelBuilder.Entity<Employee>().HasKey(e => e.EmployeeId);
            //modelBuilder.Entity<Waiter>().HasKey(w => w.EmployeeId);
            //modelBuilder.Entity<Chef>().HasKey(c => c.EmployeeId);
            //modelBuilder.Entity<BranchManager>().HasKey(bm => bm.EmployeeId);
            //modelBuilder.Entity<Cashier>().HasKey(c => c.EmployeeId);

            modelBuilder.Entity<BranchInventory>()
                .HasKey(bi => new { bi.BranchId, bi.IngredientId });

            modelBuilder.Entity<BranchMenuItem>()
                .HasKey(bm => new { bm.BranchId, bm.ItemId });

            // ❌ Ignore unsupported properties
            modelBuilder.Entity<Employee>()
                .Ignore(e => e.AssignedBranchIds);

            modelBuilder.Entity<OrderItem>()
                .Ignore(o => o.SelectedAddOnIds);

            // ✅ Basic relationships (important ones)

            modelBuilder.Entity<Order>()
                .HasOne<Customer>()
                .WithMany()
                .HasForeignKey(o => o.CustomerId);

            modelBuilder.Entity<Order>()
                .HasOne<Branches>()
                .WithMany()
                .HasForeignKey(o => o.BranchId);

            modelBuilder.Entity<OrderItem>()
                .HasOne<Order>()
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);
        }
    }
}
