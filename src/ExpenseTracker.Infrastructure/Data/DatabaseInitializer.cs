using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Enums;
using SQLite;

namespace ExpenseTracker.Infrastructure.Data;

public static class DatabaseInitializer
{
    public static async Task SeedAsync(SQLiteAsyncConnection db)
    {
        await db.CreateTableAsync<Expense>();
        if (await db.Table<Expense>().CountAsync() == 0)
        {
            var rnd = new Random();
            var cats = Enum.GetValues<ExpenseCategory>();
            var list = Enumerable.Range(1, 12).Select(i => new Expense
            {
                Amount = Math.Round((decimal)(rnd.NextDouble()*150 + 5),2),
                Category = cats[rnd.Next(cats.Length)],
                Description = $"Seed expense {i}",
                Date = DateTime.UtcNow.AddDays(-i)
            }).ToList();
            await db.InsertAllAsync(list);
        }
    }
}
