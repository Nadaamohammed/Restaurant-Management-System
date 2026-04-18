using Restaurant_Management_System.Enum;

namespace Restaurant_Management_System.Models
{
    public class Delivery
    {
        public int DeliveryId { get; set; }
        public int OrderId { get; set; }
        public int? DeliveryStaffId { get; set; }
        public DeliveryStatus Status { get; set; } = DeliveryStatus.AwaitingAssignment;
        public string DeliveryAddress { get; set; } = "";
        public DateTime? DeliveryTime { get; set; }
        public string? FailureReason { get; set; }
    }
}
