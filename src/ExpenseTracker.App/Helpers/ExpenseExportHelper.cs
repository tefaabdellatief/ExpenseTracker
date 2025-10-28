using CommunityToolkit.Maui.Storage;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.App.Helpers
{
    public static class ExpenseExportHelper
    {
        public static async Task<string> ExportExpensesToCsvAsync(IEnumerable<ExpenseDto> expenses, bool writeItAtDowns = false)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Id,Amount,Category,Date,Description,CreatedAt,ModifiedAt");

            foreach (var e in expenses)
            {
                sb.AppendLine($"" +
                    $"{e.Id}," +
                    $"{e.Amount.ToString(CultureInfo.InvariantCulture)}," +
                    $"{e.Category}," +
                    $"{e.Date:yyyy-MM-dd}," +
                    $"\"{e.Description}\"," +
                    $"{e.CreatedAt:yyyy-MM-dd HH:mm:ss}," +
                    $"{e.ModifiedAt:yyyy-MM-dd HH:mm:ss}");
            }

            var fileName = $"expenses_{DateTime.Now:yyyyMMdd_HHmmss}.csv";   
            var filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
            await File.WriteAllTextAsync(filePath, sb.ToString(), Encoding.UTF8);

            if(writeItAtDowns)
            {
                var bytes = Encoding.UTF8.GetBytes(sb.ToString());
                var result = await FileSaver.Default.SaveAsync(fileName, new MemoryStream(bytes));
                if (result.IsSuccessful)
                    await Shell.Current.DisplayAlert("Success", $"File saved: {result.FilePath}", "OK");
                else
                    await Shell.Current.DisplayAlert("Error", result.Exception?.Message ?? "Unable to save file", "OK");
            }

            return filePath;
        }
    }
}