namespace Restaurant_Management_System.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public int CustomerId { get; set; }
        public int OrderId { get; set; }
        public int Rating { get; set; } // 1 to 5
        public string Comments { get; set; } = "";
        public DateTime SubmittedAt { get; set; } = DateTime.Now;
    }
}
