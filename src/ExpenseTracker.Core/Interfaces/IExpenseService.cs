using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Interfaces;

public interface IExpenseService
{
    Task<IEnumerable<Expense>> GetAllAsync();
    Task<Expense?> GetByIdAsync(int id);
    Task AddAsync(Expense expense);
    Task UpdateAsync(Expense expense);
    Task DeleteAsync(int id);
    Task<IEnumerable<Expense>> FilterByCategoryAsync(string category);
    Task<IEnumerable<Expense>> FilterByDateRangeAsync(DateTime from, DateTime to);
}
