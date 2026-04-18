namespace Restaurant_Management_System.Models
{
    public abstract class Employee
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = "";
        public string Position { get; set; } = "";
        public decimal Salary { get; set; }
        public DateTime DateOfHire { get; set; }
        public string ContactInfo { get; set; } = "";
        public int BranchId { get; set; }
        public abstract string GetRole();
        public List<int> AssignedBranchIds { get; set; } = new();


    }
}
