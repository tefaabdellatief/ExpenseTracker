using static Entities.Enums.Enums;

namespace ExpenseTracker.App.Models;

public class CategoryExpenseData
{
    public ExpenseCategory Category { get; set; }
    public decimal Amount { get; set; }
    public double Percentage { get; set; }
    public double Progress => Percentage / 100.0; // For ProgressBar (0-1 range)
    public string DisplayText => $"{Category}: {Amount:F2} EGP ({Percentage:F1}%)";
    public string CategoryText => $"{Category}";
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
        ExpenseCategory.Food => Color.FromArgb("#f9cb9c"),        // Light palette - warm orange
        ExpenseCategory.Transport => Color.FromArgb("#0a4c47"),   // Light palette - dark teal
        ExpenseCategory.Shopping => Color.FromArgb("#c985a8"),    // Dark palette - purple
        ExpenseCategory.Entertainment => Color.FromArgb("#ea9999"), // Dark palette - coral
        ExpenseCategory.Bills => Color.FromArgb("#fce5cd"),       // Dark palette - cream
        ExpenseCategory.Other => Color.FromArgb("#fde0e1"),       // Light palette - light pink
        _ => Color.FromArgb("#0a4c47")                            // Default to primary
    };
}