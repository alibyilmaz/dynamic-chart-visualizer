using System.Collections.Generic;
using System.Threading.Tasks;
using DynamicChartApp.Domain.Enums;
using DynamicChartApp.Domain.Models;

namespace DynamicChartApp.Infrastructure.Interfaces
{
    public interface IDataRepository
    {
        Task<ExecutionResult> ExecuteAsync(ExecutionRequest req);
        Task<List<string>> ListObjectsAsync(string host, string database, string username, string password, DataObjectType type);
    }
}
