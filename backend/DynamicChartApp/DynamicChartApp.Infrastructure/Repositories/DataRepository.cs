using DynamicChartApp.Domain.Models;
using DynamicChartApp.Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;
using DynamicChartApp.Application.DTOs;

namespace DynamicChartApp.Infrastructure.Repositories;

public class DataRepository : IDataRepository
{
    public async Task<ExecutionResult> ExecuteAsync(ExecutionRequest req)
    {
        var connStr = new SqlConnectionStringBuilder
        {
            DataSource = req.Host,
            InitialCatalog = req.Database,
            UserID = req.Username,
            Password = req.Password,
            TrustServerCertificate = true
        }.ToString();

        await using var connection = new SqlConnection(connStr);
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = req.ObjectType switch
        {
            DataObjectType.Procedure => $"EXEC [{req.ObjectName}]",
            DataObjectType.Function => $"SELECT * FROM [{req.ObjectName}]()",
            DataObjectType.View => $"SELECT * FROM [{req.ObjectName}]",
            _ => throw new ArgumentException("Invalid object type")
        };
        command.CommandType = req.ObjectType == DataObjectType.Procedure
            ? System.Data.CommandType.StoredProcedure
            : System.Data.CommandType.Text;

        var reader = await command.ExecuteReaderAsync();

        var result = new ExecutionResult();

        for (int i = 0; i < reader.FieldCount; i++)
            result.Columns.Add(reader.GetName(i));

        while (await reader.ReadAsync())
        {
            var row = new Dictionary<string, object>();
            foreach (var column in result.Columns)
                row[column] = reader[column];
            result.Rows.Add(row);
        }

        return result;
    }

    public async Task<List<string>> ListObjectsAsync(string host, string database, string username, string password, DataObjectType type)
    {
        var connStr = new SqlConnectionStringBuilder
        {
            DataSource = host,
            InitialCatalog = database,
            UserID = username,
            Password = password,
            TrustServerCertificate = true
        }.ToString();

        await using var connection = new SqlConnection(connStr);
        await connection.OpenAsync();

        string sql = type switch
        {
            DataObjectType.View => "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS",
            DataObjectType.Procedure => "SELECT SPECIFIC_NAME FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'PROCEDURE'",
            DataObjectType.Function => "SELECT SPECIFIC_NAME FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'FUNCTION'",
            _ => throw new ArgumentException("Invalid object type")
        };

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
}
