namespace Restaurant_Management_System.Models
{
    public class MenuItem
    {
        public int ItemId { get; set; }
        public string Name { get; set; } = "";
        public decimal BasePrice { get; set; }
        public string Description { get; set; } = "";
        public string Category { get; set; } = "";

        // one to many --> one menu item can have many add-ons
        public List<AddOn> AddOns { get; set; } = new List<AddOn>();
    }
}
