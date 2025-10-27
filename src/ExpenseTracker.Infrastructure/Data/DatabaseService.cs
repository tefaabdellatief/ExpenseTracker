using ExpenseTracker.Core.Interfaces;
using SQLite;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Infrastructure.Data;

public class DatabaseService : IDataService
{
    private readonly SQLiteAsyncConnection _db;
    private static bool _initialized;

    public SQLiteAsyncConnection Connection => _db;

    public DatabaseService(string dbPath)
    {
        _db = new SQLiteAsyncConnection(dbPath);
    }

    public async Task InitializeAsync()
    {
        if (_initialized) return;
        await _db.CreateTableAsync<Expense>();
        _initialized = true;
    }

    public Task<int> InsertAsync<T>(T entity) where T : new() => _db.InsertAsync(entity);
    public Task<int> UpdateAsync<T>(T entity) where T : new() => _db.UpdateAsync(entity);
    public Task<int> DeleteAsync<T>(T entity) where T : new() => _db.DeleteAsync(entity);
    public Task<List<T>> GetAllAsync<T>() where T : new() => _db.Table<T>().ToListAsync();
}
