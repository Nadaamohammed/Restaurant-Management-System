namespace Restaurant_Management_System.Models
{
    public class BranchManager : Employee
    {
        public BranchManager() { Position = "Branch Manager"; }
        public override string GetRole() => "BranchManager";
    }
}

