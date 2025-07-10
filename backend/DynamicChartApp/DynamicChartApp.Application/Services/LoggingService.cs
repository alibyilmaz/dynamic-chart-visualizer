using DynamicChartApp.Domain.Models;
using DynamicChartApp.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DynamicChartApp.Application.Services;

public interface ILoggingService
{
    LogEntry CreateLogEntry(string username, string path, object? requestBody = null);
    Task SaveSuccessLogAsync(LogEntry log, object? responseBody = null);
    Task SaveErrorLogAsync(LogEntry log, string errorMessage, int statusCode = 500);
}

public class LoggingService : ILoggingService
{
    private readonly ILoggingRepository _loggingRepository;
    private readonly ILogger<LoggingService> _logger;

    public LoggingService(ILoggingRepository loggingRepository, ILogger<LoggingService> logger)
    {
        _loggingRepository = loggingRepository;
        _logger = logger;
    }

    public LogEntry CreateLogEntry(string username, string path, object? requestBody = null)
    {
        return new LogEntry
        {
            Timestamp = DateTime.UtcNow,
            Username = username,
            Path = path,
            RequestBody = requestBody != null ? JsonSerializer.Serialize(requestBody) : string.Empty,
            StatusCode = 200
        };
    }

    public async Task SaveSuccessLogAsync(LogEntry log, object? responseBody = null)
    {
        log.StatusCode = 200;
        log.ResponseBody = responseBody != null ? JsonSerializer.Serialize(responseBody) : string.Empty;
        
        await _loggingRepository.SaveLogAsync(log);
        
        _logger.LogInformation("Operation completed successfully for user {Username} at path {Path}", 
            log.Username, log.Path);
    }

    public async Task SaveErrorLogAsync(LogEntry log, string errorMessage, int statusCode = 500)
    {
        log.StatusCode = statusCode;
        log.ErrorMessage = errorMessage;
        log.ResponseBody = string.Empty;
        
        await _loggingRepository.SaveLogAsync(log);
        
        _logger.LogError("Operation failed for user {Username} at path {Path}: {ErrorMessage}", 
            log.Username, log.Path, errorMessage);
    }
}
