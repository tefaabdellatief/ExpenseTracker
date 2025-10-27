
using Entities.Dtos;
using SQLite;
using static Entities.Enums.Enums;

namespace ExpenseTracker.Infrastructure.SqliteConfigs
{
    public static class Initializer
    {
        public static async Task SeedAsync(SQLiteAsyncConnection db)
        {
            await db.CreateTableAsync<ExpenseDto>();
            if (await db.Table<ExpenseDto>().CountAsync() == 0)
            {
                var rnd = new Random();
                var cats = Enum.GetValues<ExpenseCategory>();
                var list = Enumerable.Range(1, 12).Select(i => new ExpenseDto
                {
                    Amount = Math.Round((decimal)(rnd.NextDouble() * 150 + 9), 2),
                    Category = cats[rnd.Next(cats.Length)],
                    Description = $"test expense {i}",
                    Date = DateTime.UtcNow.AddDays(-i)
                }).ToList();
                await db.InsertAllAsync(list);
            }
        }
    }
}