namespace DynamicChartApp.Application.DTOs;

/// <summary>
/// Request DTO for executing a database object (view, procedure, function).
/// </summary>
public class ExecutionRequestDto
{
    public string Host { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ObjectName { get; set; } = string.Empty;
    public DataObjectType ObjectType { get; set; }
}

/// <summary>
/// Request DTO for listing database objects of a given type.
/// </summary>
public class ListObjectsRequestDto
{
    public string Host { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DataObjectType Type { get; set; }
}
