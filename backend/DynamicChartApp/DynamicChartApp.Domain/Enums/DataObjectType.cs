using System.Text.Json.Serialization;

namespace DynamicChartApp.Domain.Enums;

/// <summary>
/// Represents the type of database object.
/// </summary>
public enum DataObjectType
{
    /// <summary>Database view</summary>
    [JsonPropertyName("View")]
    View,
    /// <summary>Stored procedure</summary>
    [JsonPropertyName("SP")]
    Procedure,
    /// <summary>Table-valued or scalar function</summary>
    [JsonPropertyName("Function")]
    Function
}
