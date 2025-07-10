namespace DynamicChartApp.Domain.Exceptions;

/// <summary>
/// Base exception for all application-specific exceptions
/// </summary>
public abstract class DynamicChartAppException : Exception
{
    protected DynamicChartAppException(string message) : base(message) { }
    protected DynamicChartAppException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when database connection fails
/// </summary>
public class DatabaseConnectionException : DynamicChartAppException
{
    public DatabaseConnectionException(string message) : base(message) { }
    public DatabaseConnectionException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when database object is not found or invalid
/// </summary>
public class DatabaseObjectException : DynamicChartAppException
{
    public DatabaseObjectException(string message) : base(message) { }
    public DatabaseObjectException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when authentication fails
/// </summary>
public class AuthenticationException : DynamicChartAppException
{
    public AuthenticationException(string message) : base(message) { }
    public AuthenticationException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when validation fails
/// </summary>
public class ValidationException : DynamicChartAppException
{
    public ValidationException(string message) : base(message) { }
    public ValidationException(string message, Exception innerException) : base(message, innerException) { }
}
