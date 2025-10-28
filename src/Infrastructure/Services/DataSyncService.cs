

using Entities.Dtos;
using ExpenseTracker.Infrastructure.Interfaces;

namespace ExpenseTracker.Infrastructure.Services;

public class DataSyncService
{
    private readonly IExpenseService _remote;
    private readonly IDataService _local;

    public DataSyncService(IExpenseService remote, IDataService local)
    {
        _remote = remote;
        _local = local;
    }

    public async Task SyncAsync()
    {
        try
        {
            var local = await _local.GetAllAsync<ExpenseDto>();
            var remote = await _remote.GetAllAsync();

            var missing = remote.Where(r => !local.Any(l => l.Id == r.Id)).ToList();
            foreach (var r in missing)
            {
                await _local.InsertAsync(r);
            }
        }
        catch (Exception)
        {
            return;
        }
    }
}