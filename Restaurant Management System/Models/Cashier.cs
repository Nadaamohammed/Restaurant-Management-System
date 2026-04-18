namespace Restaurant_Management_System.Models
{
    public class Cashier : Employee
    {
        public Cashier() { Position = "Cashier"; }
        public override string GetRole() => "Cashier";
    }
}
