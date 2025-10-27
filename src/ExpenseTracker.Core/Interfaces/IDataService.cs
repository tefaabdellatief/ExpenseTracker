namespace ExpenseTracker.Core.Interfaces;

public interface IDataService
{
    Task InitializeAsync();
    Task<int> InsertAsync<T>(T entity) where T : new();
    Task<int> UpdateAsync<T>(T entity) where T : new();
    Task<int> DeleteAsync<T>(T entity) where T : new();
    Task<List<T>> GetAllAsync<T>() where T : new();
}
