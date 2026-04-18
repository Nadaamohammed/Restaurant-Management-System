namespace Restaurant_Management_System.Models
{
    public class MenuItem
    {
        public int ItemId { get; set; }
        public string Name { get; set; } = "";
        public decimal BasePrice { get; set; }
        public string Description { get; set; } = "";
        public string Category { get; set; } = "";
        public List<AddOn> AddOns { get; set; } = new List<AddOn>();
    }
}
