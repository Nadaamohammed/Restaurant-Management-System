using Restaurant_Management_System;
using Restaurant_Management_System.Enum;
using Restaurant_Management_System.Models;

public class FeedbackService
{
    private readonly ResaturantContext _context;

    public FeedbackService(ResaturantContext context)
    {
        _context = context;
    }

    public (bool Success, string Message) Submit(
        int customerId, int orderId, int rating, string comments)
    {
        var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);

        if (order is null)
            return (false, "Order not found.");

        if (order.CustomerId != customerId)
            return (false, "Invalid customer.");

        if (order.Status != OrderStatus.Completed)
            return (false, "Only completed orders can be reviewed.");

        if (_context.Feedbacks.Any(f => f.CustomerId == customerId && f.OrderId == orderId))
            return (false, "Feedback already exists.");

        if (rating < 1 || rating > 5)
            return (false, "Rating must be between 1 and 5.");

        var feedback = new Feedback
        {
            CustomerId = customerId,
            OrderId = orderId,
            SubmittedAt = DateTime.Now,
            Rating = rating,
            Comments = string.IsNullOrWhiteSpace(comments)
                ? "No comments"
                : comments.Trim()
        };

        _context.Feedbacks.Add(feedback);
        _context.SaveChanges();

        return (true, "Feedback saved.");
    }
}