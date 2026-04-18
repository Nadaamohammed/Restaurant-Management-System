namespace Restaurant_Management_System.Models
{
    public class Waiter : Employee
    {
        public Waiter() { Position = "Waiter"; }
        public override string GetRole() => "Waiter";
    }
}
