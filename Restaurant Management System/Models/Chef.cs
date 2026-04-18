namespace Restaurant_Management_System.Models
{
    public class Chef : Employee
    {
        public Chef() { Position = "Chef"; }
        public override string GetRole() => "Chef";
    }
}
