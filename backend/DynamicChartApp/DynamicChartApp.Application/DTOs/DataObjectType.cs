namespace DynamicChartApp.Application.DTOs;

/// <summary>
/// Represents the type of database object.
/// </summary>
public enum DataObjectType
{
    /// <summary>Database view</summary>
    View,
    /// <summary>Stored procedure</summary>
    Procedure,
    /// <summary>Table-valued or scalar function</summary>
    Function
} 