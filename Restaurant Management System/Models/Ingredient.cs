namespace Restaurant_Management_System.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public string Name { get; set; } = "";
        //kg, liter, piece, etc.
        public string Unit { get; set; } = "";
    }
}
