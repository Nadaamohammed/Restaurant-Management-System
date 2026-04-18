namespace Restaurant_Management_System.Models
{
    public class ShiftSchedule
    {
        public int ScheduleId { get; set; }
        public int BranchId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public string TimeSlot { get; set; } = "";
    }
}
