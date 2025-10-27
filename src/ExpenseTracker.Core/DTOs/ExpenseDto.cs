namespace ExpenseTracker.Core.DTOs;

public record ExpenseDto(int Id, decimal Amount, string Category, DateTime Date, string? Description);