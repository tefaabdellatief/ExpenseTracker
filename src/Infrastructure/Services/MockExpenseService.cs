

using Entities.Dtos;
using ExpenseTracker.Infrastructure.Interfaces;
using static Entities.Enums.Enums;

namespace ExpenseTracker.Infrastructure.Services;

public class MockExpenseService : IExpenseService
{
    private readonly List<ExpenseDto> _expenses = new();
    private int _nextId = 1;

    public MockExpenseService()
    {
        var rnd = new Random();
        var cats = Enum.GetValues<ExpenseCategory>();
        for (int i = 0; i < 6; i++)
        {
            _expenses.Add(new ExpenseDto
            {
                Id = _nextId++,
                Amount = Math.Round((decimal)(rnd.NextDouble()*200 + 5), 2),
                Category = cats[rnd.Next(cats.Length)],
                Date = DateTime.UtcNow.AddDays(-rnd.Next(0,30)),
                Description = $"Sample expense {i+1}",
                CreatedAt = DateTime.UtcNow.AddDays(-rnd.Next(0,30))
            });
        }
    }

    private static Task Delay() => Task.Delay(Random.Shared.Next(300,500));

    public async Task AddAsync(ExpenseDto expense)
    {
        await Delay();
        expense.Id = _nextId++;
        _expenses.Add(expense);
    }

    public async Task DeleteAsync(int id)
    {
        await Delay();
        _expenses.RemoveAll(e => e.Id == id);
    }

    public async Task<IEnumerable<ExpenseDto>> FilterByCategoryAsync(string category)
    {
        await Delay();
        return _expenses.Where(e => e.Category.ToString() == category).OrderByDescending(e=>e.Date);
    }

    public async Task<IEnumerable<ExpenseDto>> FilterByDateRangeAsync(DateTime from, DateTime to)
    {
        await Delay();
        return _expenses.Where(e => e.Date >= from && e.Date <= to).OrderByDescending(e=>e.Date);
    }

    public async Task<IEnumerable<ExpenseDto>> FilterByCategoryAndDateRangeAsync(string category, DateTime from, DateTime to)
    {
        await Delay();
        return _expenses.Where(e => (e.Date >= from && e.Date <= to) && e.Category.Equals(category)).OrderByDescending(e => e.Date);
    }

    public async Task<IEnumerable<ExpenseDto>> GetAllAsync()
    {
        await Delay();
        return _expenses.OrderByDescending(e=>e.Date);
    }

    public async Task<ExpenseDto?> GetByIdAsync(int id)
    {
        await Delay();
        return _expenses.FirstOrDefault(e => e.Id == id);
    }

    public async Task UpdateAsync(ExpenseDto expense)
    {
        await Delay();
        var ex = _expenses.FirstOrDefault(e => e.Id == expense.Id);
        if (ex != null)
        {
            ex.Amount = expense.Amount;
            ex.Category = expense.Category;
            ex.Description = expense.Description;
            ex.Date = expense.Date;
            ex.ModifiedAt = DateTime.UtcNow;
        }
    }
}