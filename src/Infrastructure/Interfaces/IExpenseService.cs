using Entities.Dtos;

namespace ExpenseTracker.Infrastructure.Interfaces;

public interface IExpenseService
{
    public Task<IEnumerable<ExpenseDto>> GetAllAsync();
    public Task<ExpenseDto?> GetByIdAsync(int id);
    public Task AddAsync(ExpenseDto expense);
    public Task UpdateAsync(ExpenseDto expense);
    public Task DeleteAsync(int id);
    public Task<IEnumerable<ExpenseDto>> FilterByCategoryAsync(string category);
    public Task<IEnumerable<ExpenseDto>> FilterByDateRangeAsync(DateTime from, DateTime to);
    public Task<IEnumerable<ExpenseDto>> FilterByCategoryAndDateRangeAsync(string category, DateTime from, DateTime to);
}
