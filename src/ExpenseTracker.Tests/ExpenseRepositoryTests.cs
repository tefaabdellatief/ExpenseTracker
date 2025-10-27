using ExpenseTracker.Infrastructure.Data;
using System.IO;
using Xunit;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Tests;

public class ExpenseRepositoryTests
{
    [Fact]
    public async Task AddAndGetAll_Works()
    {
        var dbPath = Path.Combine(Path.GetTempPath(), "unit_test_expenses.db3");
        if (File.Exists(dbPath)) File.Delete(dbPath);

        var ds = new DatabaseService(dbPath);
        await ds.InitializeAsync();
        await ds.InsertAsync(new Expense { Amount = 12.5m, Category = ExpenseTracker.Core.Enums.ExpenseCategory.Food, Description = "test" });

        var all = await ds.GetAllAsync<Expense>();
        Assert.Single(all);
        Assert.Equal(12.5m, all[0].Amount);
    }
}