using DynamicChartApp.Domain.Common;
using DynamicChartApp.Domain.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace DynamicChartApp.Infrastructure.Services;

public interface IConnectionService
{
    Result<SqlConnection> CreateConnection(string host, string database, string username, string password);
    Task<Result<SqlConnection>> CreateAndOpenConnectionAsync(string host, string database, string username, string password);
}

public class ConnectionService : IConnectionService
{
    private readonly ILogger<ConnectionService> _logger;

    public ConnectionService(ILogger<ConnectionService> logger)
    {
        _logger = logger;
    }

    public Result<SqlConnection> CreateConnection(string host, string database, string username, string password)
    {
        try
        {
            var connectionString = new SqlConnectionStringBuilder
            {
                DataSource = host,
                InitialCatalog = database,
                UserID = username,
                Password = password,
                TrustServerCertificate = true,
                ConnectTimeout = 30,
                CommandTimeout = 300
            }.ToString();

            var connection = new SqlConnection(connectionString);
            return Result<SqlConnection>.Success(connection);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create database connection for host {Host}, database {Database}", host, database);
            return Result<SqlConnection>.Failure($"Failed to create connection: {ex.Message}");
        }
    }

    public async Task<Result<SqlConnection>> CreateAndOpenConnectionAsync(string host, string database, string username, string password)
    {
        var connectionResult = CreateConnection(host, database, username, password);
        
        if (connectionResult.IsFailure)
            return connectionResult;

        try
        {
            await connectionResult.Value!.OpenAsync();
            return connectionResult;
        }
        catch (SqlException ex)
        {
            connectionResult.Value?.Dispose();
            _logger.LogError(ex, "Failed to open database connection for host {Host}, database {Database}", host, database);
            
            var errorMessage = ex.Number switch
            {
                2 => "Server not found or network not accessible",
                18456 => "Authentication failed",
                4060 => "Database not found",
                _ => $"Database connection failed: {ex.Message}"
            };
            
            return Result<SqlConnection>.Failure(errorMessage);
        }
        catch (Exception ex)
        {
            connectionResult.Value?.Dispose();
            _logger.LogError(ex, "Unexpected error opening database connection for host {Host}, database {Database}", host, database);
            return Result<SqlConnection>.Failure($"Unexpected connection error: {ex.Message}");
        }
    }
}
