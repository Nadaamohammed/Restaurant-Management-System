namespace Restaurant_Management_System.Models
{
    // branch , ingredient --> many to many 
    public class BranchInventory
    {
        public int BranchId { get; set; }
        public int IngredientId { get; set; }
        public double CurrentQuantity { get; set; }
    }
}
