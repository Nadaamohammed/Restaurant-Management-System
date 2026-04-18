namespace Restaurant_Management_System.Models
{
    public class BranchMenuItem
    {
        public int ItemId { get; set; }
        public int BranchId { get; set; }
        public bool IsAvailable { get; set; } = true;
        public decimal? PriceOverride { get; set; }

    }
}
