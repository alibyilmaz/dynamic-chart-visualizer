using DynamicChartApp.Application.DTOs;
using DynamicChartApp.Application.ResponseModel;
using DynamicChartApp.Domain.Exceptions;
using DynamicChartApp.Domain.Models;
using DynamicChartApp.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DynamicChartApp.Application.Services;

public interface IErrorHandlingService
{
    Task<ResponseDto<T>> HandleErrorAsync<T>(
        Exception ex, 
        LogEntry log, 
        string operation, 
        string username = "",
        string path = "");

    Task<ResponseDto> HandleErrorAsync(
        Exception ex, 
        LogEntry log, 
        string operation, 
        string username = "",
        string path = "");
}

public class ErrorHandlingService : IErrorHandlingService
{
    private readonly ILoggingRepository _loggingRepository;
    private readonly ILogger<ErrorHandlingService> _logger;

    public ErrorHandlingService(ILoggingRepository loggingRepository, ILogger<ErrorHandlingService> logger)
    {
        _loggingRepository = loggingRepository;
        _logger = logger;
    }

    public async Task<ResponseDto<T>> HandleErrorAsync<T>(
        Exception ex, 
        LogEntry log, 
        string operation, 
        string username = "",
        string path = "")
    {
        var (statusCode, message) = GetErrorDetails(ex);
        
        log.StatusCode = statusCode;
        log.ErrorMessage = ex.Message;
        log.ResponseBody = string.Empty;

        await _loggingRepository.SaveLogAsync(log);
        
        _logger.LogError(ex, "Error in operation {Operation} for user {Username} at path {Path}", 
            operation, username, path);

        return new ResponseDto<T>
        {
            Data = default,
            Status = "Error",
            Message = message,
            Execution = null
        };
    }

    public async Task<ResponseDto> HandleErrorAsync(
        Exception ex, 
        LogEntry log, 
        string operation, 
        string username = "",
        string path = "")
    {
        var (statusCode, message) = GetErrorDetails(ex);
        
        log.StatusCode = statusCode;
        log.ErrorMessage = ex.Message;
        log.ResponseBody = string.Empty;

        await _loggingRepository.SaveLogAsync(log);
        
        _logger.LogError(ex, "Error in operation {Operation} for user {Username} at path {Path}", 
            operation, username, path);

        return new ResponseDto
        {
            Data = null,
            Status = "Error",
            Message = message,
            Execution = null
        };
    }

    private static (int StatusCode, string Message) GetErrorDetails(Exception ex)
    {
        return ex switch
        {
            ValidationException => (400, ex.Message),
            AuthenticationException => (401, "Authentication failed"),
            DatabaseConnectionException => (503, "Database connection failed"),
            DatabaseObjectException => (404, ex.Message),
            ArgumentException => (400, ex.Message),
            UnauthorizedAccessException => (401, "Unauthorized access"),
            _ => (500, "An internal server error occurred")
        };
    }
}
