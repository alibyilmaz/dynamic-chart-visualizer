using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DynamicChartApp.Application.DTOs;
using DynamicChartApp.Domain.Models;
using DynamicChartApp.Infrastructure.Interfaces;
using DynamicChartApp.Application.ResponseModel;
using DynamicChartApp.Domain.Enums;
using DynamicChartApp.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace DynamicChartApp.Application.Services;

public class DataService
{
    private readonly IDataRepository _dataRepository;
    private readonly IValidationService _validationService;
    private readonly ILoggingService _loggingService;
    private readonly IErrorHandlingService _errorHandlingService;
    private readonly ILogger<DataService> _logger;

    public DataService(
        IDataRepository dataRepository,
        IValidationService validationService,
        ILoggingService loggingService,
        IErrorHandlingService errorHandlingService,
        ILogger<DataService> logger)
    {
        _dataRepository = dataRepository;
        _validationService = validationService;
        _loggingService = loggingService;
        _errorHandlingService = errorHandlingService;
        _logger = logger;
    }

    /// <summary>
    /// Executes a database object and logs the request/response.
    /// </summary>
    /// <param name="dto">Execution request DTO</param>
    /// <param name="path">API endpoint path</param>
    /// <returns>ResponseDto with execution result and metadata</returns>
    public async Task<ResponseDto> ExecuteAndLogAsync(ExecutionRequestDto dto, string path)
    {
        var log = _loggingService.CreateLogEntry(dto.Username, path, dto);

        try
        {
            // Validate request
            var validationResult = _validationService.ValidateExecutionRequest(dto);
            if (validationResult.IsFailure)
            {
                throw new ValidationException(validationResult.Error!);
            }

            var request = new ExecutionRequest
            {
                Host = dto.Host,
                Database = dto.Database,
                Username = dto.Username,
                Password = dto.Password,
                ObjectName = dto.ObjectName,
                ObjectType = dto.ObjectType
            };

            var result = await _dataRepository.ExecuteAsync(request);

            var responseData = new ExecutionResultDto
            {
                Columns = result.Columns,
                Rows = result.Rows
            };

            await _loggingService.SaveSuccessLogAsync(log, responseData);

            var finishedAt = DateTime.UtcNow;
            return new ResponseDto
            {
                Data = responseData,
                Status = "Success",
                Message = "Execution completed successfully.",
                Execution = new ExecutionMetaDto
                {
                    StartedAt = log.Timestamp,
                    FinishedAt = finishedAt,
                    DurationMs = (int)(finishedAt - log.Timestamp).TotalMilliseconds
                }
            };
        }
        catch (Exception ex)
        {
            return await _errorHandlingService.HandleErrorAsync(ex, log, "Execute", dto.Username, path);
        }
    }

    /// <summary>
    /// Lists database objects of the specified type.
    /// </summary>
    public async Task<ResponseDto<List<string>>> ListObjectsAndLogAsync(string host, string database, string username, string password, DataObjectType type, string path)
    {
        var requestData = new { host, database, username, password, type };
        var log = _loggingService.CreateLogEntry(username, path, requestData);

        try
        {
            // Validate request
            var validationResult = _validationService.ValidateListObjectsRequest(host, database, username, password, type);
            if (validationResult.IsFailure)
            {
                throw new ValidationException(validationResult.Error!);
            }

            var result = await _dataRepository.ListObjectsAsync(host, database, username, password, type);
            
            await _loggingService.SaveSuccessLogAsync(log, result);
            
            return new ResponseDto<List<string>>
            {
                Data = result,
                Status = "Success",
                Message = "Objects listed successfully.",
                Execution = null
            };
        }
        catch (Exception ex)
        {
            return await _errorHandlingService.HandleErrorAsync<List<string>>(ex, log, "ListObjects", username, path);
        }
    }
}
