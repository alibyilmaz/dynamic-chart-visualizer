using System.Collections.Generic;
using System.Threading.Tasks;
using DynamicChartApp.Application.DTOs;

namespace DynamicChartApp.Infrastructure.Interfaces
{
    public interface IDataRepository
    {
        Task<ExecutionResult> ExecuteAsync(ExecutionRequest req);
        Task<List<string>> ListObjectsAsync(string host, string database, string username, string password, DataObjectType type);
    }
}
