using Restaurant_Management_System.Enum;

namespace Restaurant_Management_System.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public OrderType Type { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; } = 0;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime OrderDateTime { get; set; } = DateTime.Now;
        public int BranchId { get; set; }
        public int CustomerId { get; set; }
        public string? DeliveryAddress { get; set; }
        public int HandledByEmployeeId { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    }
}
