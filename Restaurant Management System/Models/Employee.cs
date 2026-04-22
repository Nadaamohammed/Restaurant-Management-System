namespace Restaurant_Management_System.Models
{
    /// <summary>
    /// every employee has a role, 
    /// such as manager, chef, waiter, etc.
    /// This class serves as a base for all employee types, 
    /// allowing for common properties and methods to be defined while enabling specific roles to implement their unique behaviors.
    /// overriding the GetRole method in derived classes allows for easy identification of an employee's role,
    /// which can be useful for role-based access control, task assignment, and other functionalities within the restaurant management system.
    public abstract class Employee
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = "";
        public string Position { get; set; } = "";
        public decimal Salary { get; set; }
        public DateTime DateOfHire { get; set; }
        public string ContactInfo { get; set; } = "";
        public abstract string GetRole();



        public int BranchId { get; set; }
        public List<int> AssignedBranchIds { get; set; } = new();


    }
}
