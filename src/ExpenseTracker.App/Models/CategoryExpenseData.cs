using static Entities.Enums.Enums;

namespace ExpenseTracker.App.Models;

public class CategoryExpenseData
{
    public ExpenseCategory Category { get; set; }
    public decimal Amount { get; set; }
    public double Percentage { get; set; }
    public double Progress => Percentage / 100.0; // For ProgressBar (0-1 range)
    public string DisplayText => $"{Category}: {Amount:F2} EGP ({Percentage:F1}%)";
    public string CategoryIcon => GetCategoryIcon(Category);
    public Color CategoryColor => GetCategoryColor(Category);

    private static string GetCategoryIcon(ExpenseCategory category) => category switch
    {
        ExpenseCategory.Food => "🍽️",
        ExpenseCategory.Transport => "🚗",
        ExpenseCategory.Shopping => "🛍️",
        ExpenseCategory.Entertainment => "🎬",
        ExpenseCategory.Bills => "📄",
        ExpenseCategory.Other => "📦",
        _ => "💰"
    };

    private static Color GetCategoryColor(ExpenseCategory category) => category switch
    {
        ExpenseCategory.Food => Colors.Orange,
        ExpenseCategory.Transport => Colors.Blue,
        ExpenseCategory.Shopping => Colors.Purple,
        ExpenseCategory.Entertainment => Colors.Red,
        ExpenseCategory.Bills => Colors.Green,
        ExpenseCategory.Other => Colors.Gray,
        _ => Colors.Black
    };
}