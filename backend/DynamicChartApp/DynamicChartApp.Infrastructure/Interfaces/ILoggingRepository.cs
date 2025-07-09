using DynamicChartApp.Domain.Models;


namespace DynamicChartApp.Infrastructure.Interfaces
{
    public interface ILoggingRepository
    {
        Task SaveLogAsync(LogEntry log);
    }
}
