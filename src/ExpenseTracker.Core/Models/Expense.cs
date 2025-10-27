using ExpenseTracker.Core.Enums;

namespace ExpenseTracker.Core.Models;

public class Expense
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public ExpenseCategory Category { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedAt { get; set; }
}