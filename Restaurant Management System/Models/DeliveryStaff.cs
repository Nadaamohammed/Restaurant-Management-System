namespace Restaurant_Management_System.Models
{
    public class DeliveryStaff
    {
        public int StaffId { get; set; }
        public string FullName { get; set; } = "";
        public string VehicleType { get; set; } = "";
        public string LicenseNumber { get; set; } = "";
        public int BranchId { get; set; }
        public bool IsAvailable { get; set; } = true;
        public string AssignedArea { get; set; } = "";
    }
}
