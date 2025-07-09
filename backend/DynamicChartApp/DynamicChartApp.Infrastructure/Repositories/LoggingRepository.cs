using DynamicChartApp.Domain.Models;
using DynamicChartApp.Infrastructure.Data;
using DynamicChartApp.Infrastructure.Interfaces;

namespace DynamicChartApp.Infrastructure.Repositories;

public class LoggingRepository : ILoggingRepository
{
    private readonly AppDbContext _context;

    public LoggingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveLogAsync(LogEntry log)
    {
        _context.Logs.Add(log);
        await _context.SaveChangesAsync();
    }
}
