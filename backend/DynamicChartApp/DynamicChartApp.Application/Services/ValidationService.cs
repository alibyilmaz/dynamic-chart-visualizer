using DynamicChartApp.Application.DTOs;
using DynamicChartApp.Domain.Common;
using DynamicChartApp.Domain.Enums;
using DynamicChartApp.Domain.Exceptions;

namespace DynamicChartApp.Application.Services;

public interface IValidationService
{
    Result ValidateExecutionRequest(ExecutionRequestDto request);
    Result ValidateListObjectsRequest(string host, string database, string username, string password, DataObjectType type);
}

public class ValidationService : IValidationService
{
    public Result ValidateExecutionRequest(ExecutionRequestDto request)
    {
        if (request == null)
            return Result.Failure("Request cannot be null");

        if (string.IsNullOrWhiteSpace(request.Host))
            return Result.Failure("Host is required");

        if (string.IsNullOrWhiteSpace(request.Database))
            return Result.Failure("Database is required");

        if (string.IsNullOrWhiteSpace(request.Username))
            return Result.Failure("Username is required");

        if (string.IsNullOrWhiteSpace(request.Password))
            return Result.Failure("Password is required");

        if (string.IsNullOrWhiteSpace(request.ObjectName))
            return Result.Failure("Object name is required");

        if (!Enum.IsDefined(typeof(DataObjectType), request.ObjectType))
            return Result.Failure("Invalid object type");

        // Basic SQL injection prevention
        if (ContainsSqlInjectionPatterns(request.ObjectName))
            return Result.Failure("Object name contains invalid characters");

        return Result.Success();
    }

    public Result ValidateListObjectsRequest(string host, string database, string username, string password, DataObjectType type)
    {
        if (string.IsNullOrWhiteSpace(host))
            return Result.Failure("Host is required");

        if (string.IsNullOrWhiteSpace(database))
            return Result.Failure("Database is required");

        if (string.IsNullOrWhiteSpace(username))
            return Result.Failure("Username is required");

        if (string.IsNullOrWhiteSpace(password))
            return Result.Failure("Password is required");

        if (!Enum.IsDefined(typeof(DataObjectType), type))
            return Result.Failure("Invalid object type");

        return Result.Success();
    }

    private static bool ContainsSqlInjectionPatterns(string input)
    {
        var dangerousPatterns = new[]
        {
            "'", "\"", ";", "--", "/*", "*/", "xp_cmdshell", "xp_", 
            " exec ", " execute ", " select ", " insert ", " update ", 
            " delete ", " drop ", " create ", " alter ", "union select",
            "script>", "<script", "javascript:", "vbscript:"
        };

        return dangerousPatterns.Any(pattern => 
            input.Contains(pattern, StringComparison.OrdinalIgnoreCase));
    }
}
