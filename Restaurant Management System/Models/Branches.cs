namespace Restaurant_Management_System.Models
{
    public class Branches
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; } = "";
        public string BranchAddress { get; set; }= "";
        public string ContactNumber { get; set; } = "";
        public string OpeningHours { get; set; } = "";
        public int ManagerId { get; set; }
    }
}
