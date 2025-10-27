namespace ExpenseTracker.Infrastructure.Interfaces;

public interface IDataService
{
   public Task InitializeAsync();
   public Task<int> InsertAsync<T>(T entity) where T : new();
   public Task<int> UpdateAsync<T>(T entity) where T : new();
   public Task<int> DeleteAsync<T>(T entity) where T : new();
   public Task<List<T>> GetAllAsync<T>() where T : new();
}