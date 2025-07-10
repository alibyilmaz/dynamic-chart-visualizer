using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DynamicChartApp.Domain.Models;
using DynamicChartApp.Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;
using DynamicChartApp.Domain.Enums;
using DynamicChartApp.Infrastructure.Services;
using DynamicChartApp.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace DynamicChartApp.Infrastructure.Repositories;

public class DataRepository : IDataRepository
{
    private readonly IConnectionService _connectionService;
    private readonly ILogger<DataRepository> _logger;

    public DataRepository(IConnectionService connectionService, ILogger<DataRepository> logger)
    {
        _connectionService = connectionService;
        _logger = logger;
    }

    public async Task<ExecutionResult> ExecuteAsync(ExecutionRequest req)
    {
        var connectionResult = await _connectionService.CreateAndOpenConnectionAsync(
            req.Host, req.Database, req.Username, req.Password);

        if (connectionResult.IsFailure)
        {
            throw new DatabaseConnectionException(connectionResult.Error!);
        }

        await using var connection = connectionResult.Value!;

        try
        {
            var command = connection.CreateCommand();
            ConfigureCommand(command, req);

            var reader = await command.ExecuteReaderAsync();
            return await BuildExecutionResultAsync(reader);
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL execution failed for object {ObjectName} of type {ObjectType}", 
                req.ObjectName, req.ObjectType);
            throw new DatabaseObjectException($"Failed to execute {req.ObjectType} '{req.ObjectName}': {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error executing object {ObjectName} of type {ObjectType}", 
                req.ObjectName, req.ObjectType);
            throw new DatabaseObjectException($"Unexpected error executing {req.ObjectType} '{req.ObjectName}': {ex.Message}", ex);
        }
    }

    public async Task<List<string>> ListObjectsAsync(string host, string database, string username, string password, DataObjectType type)
    {
        var connectionResult = await _connectionService.CreateAndOpenConnectionAsync(
            host, database, username, password);

        if (connectionResult.IsFailure)
        {
            throw new DatabaseConnectionException(connectionResult.Error!);
        }

        await using var connection = connectionResult.Value!;

        try
        {
            string sql = GetListObjectsQuery(type);
            
            var command = connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = System.Data.CommandType.Text;

            var reader = await command.ExecuteReaderAsync();
            var result = new List<string>();
            
            while (await reader.ReadAsync())
            {
                result.Add(reader.GetString(0));
            }
            
            return result;
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL query failed for listing {ObjectType} objects", type);
            throw new DatabaseObjectException($"Failed to list {type} objects: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error listing {ObjectType} objects", type);
            throw new DatabaseObjectException($"Unexpected error listing {type} objects: {ex.Message}", ex);
        }
    }

    private static void ConfigureCommand(SqlCommand command, ExecutionRequest req)
    {
        // Configure command based on object type
        command.CommandText = req.ObjectType switch
        {
            DataObjectType.Procedure => req.ObjectName, // Just the procedure name for StoredProcedure CommandType
            DataObjectType.Function => GetFunctionQuery(req.ObjectName, req.Parameters), // Handle function calls properly
            DataObjectType.View => $"SELECT * FROM dbo.[{req.ObjectName}]", // Include schema for views
            _ => throw new ArgumentException("Invalid object type")
        };
        
        command.CommandType = req.ObjectType == DataObjectType.Procedure
            ? System.Data.CommandType.StoredProcedure
            : System.Data.CommandType.Text;
    }

    private static string GetFunctionQuery(string functionName, Dictionary<string, object>? parameters)
    {
        // Handle specific known functions with their parameters
        return parameters != null && parameters.TryGetValue("functionName", out var dynamicFunctionName)
            ? $"SELECT * FROM dbo.[{dynamicFunctionName}]()"
            : throw new ArgumentException("Missing required parameter: functionName");
    }

    private static async Task<ExecutionResult> BuildExecutionResultAsync(SqlDataReader reader)
    {
        var result = new ExecutionResult();

        // Get column names
        for (int i = 0; i < reader.FieldCount; i++)
        {
            result.Columns.Add(reader.GetName(i));
        }

        // Read data rows
        while (await reader.ReadAsync())
        {
            var row = new Dictionary<string, object>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                var value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                row[reader.GetName(i)] = value ?? "";
            }
            result.Rows.Add(row);
        }

        return result;
    }

    private static string GetListObjectsQuery(DataObjectType type)
    {
        return type switch
        {
            DataObjectType.View => "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS",
            DataObjectType.Procedure => "SELECT SPECIFIC_NAME FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'PROCEDURE'",
            DataObjectType.Function => "SELECT SPECIFIC_NAME FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'FUNCTION'",
            _ => throw new ArgumentException("Invalid object type")
        };
    }
}
