using System.Diagnostics;
using System.Text.Json;
using DynamicChartApp.Application.DTOs;
using DynamicChartApp.Domain.Models;
using DynamicChartApp.Infrastructure.Interfaces;
using DynamicChartApp.Application.ResponseModel;

namespace DynamicChartApp.Application.Services;

public class DataService
{
    private readonly IDataRepository _dataRepository;
    private readonly ILoggingRepository _loggingRepository;

    public DataService(IDataRepository dataRepository, ILoggingRepository loggingRepository)
    {
        _dataRepository = dataRepository;
        _loggingRepository = loggingRepository;
    }

    public async Task<ResponseDto> ExecuteAndLogAsync(ExecutionRequestDto dto, string path)
    {
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

        var maskedRequest = new ExecutionRequest
        {
            Host = request.Host,
            Database = request.Database,
            Username = request.Username,
            Password = "***",
            ObjectName = request.ObjectName,
            ObjectType = request.ObjectType
        };

        var log = new LogEntry
        {
            Timestamp = DateTime.UtcNow,
            Username = request.Username,
            Path = path,
            RequestBody = JsonSerializer.Serialize(maskedRequest),
            ResponseBody = JsonSerializer.Serialize(result),
            StatusCode = 200
        };

        await _loggingRepository.SaveLogAsync(log);
        var finishedAt = DateTime.UtcNow;
        return new ResponseDto
        {
            Data = new ExecutionResultDto
            {
                Columns = result.Columns,
                Rows = result.Rows
            },
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

    public async Task<List<string>> ListObjectsAsync(string host, string database, string username, string password, DataObjectType type)
    {
        return await _dataRepository.ListObjectsAsync(host, database, username, password, type);
    }
}
