using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Enums;

namespace ExpenseTracker.Infrastructure.Services;

public class MockExpenseService : IExpenseService
{
    private readonly List<Expense> _expenses = new();
    private int _nextId = 1;

    public MockExpenseService()
    {
        var rnd = new Random();
        var cats = Enum.GetValues<ExpenseCategory>();
        for (int i = 0; i < 20; i++)
        {
            _expenses.Add(new Expense
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

    public async Task AddAsync(Expense expense)
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

    public async Task<IEnumerable<Expense>> FilterByCategoryAsync(string category)
    {
        await Delay();
        return _expenses.Where(e => e.Category.ToString() == category).OrderByDescending(e=>e.Date);
    }

    public async Task<IEnumerable<Expense>> FilterByDateRangeAsync(DateTime from, DateTime to)
    {
        await Delay();
        return _expenses.Where(e => e.Date >= from && e.Date <= to).OrderByDescending(e=>e.Date);
    }

    public async Task<IEnumerable<Core.Models.Expense>> GetAllAsync()
    {
        await Delay();
        return _expenses.OrderByDescending(e=>e.Date);
    }

    public async Task<Core.Models.Expense?> GetByIdAsync(int id)
    {
        await Delay();
        return _expenses.FirstOrDefault(e => e.Id == id);
    }

    public async Task UpdateAsync(Core.Models.Expense expense)
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
