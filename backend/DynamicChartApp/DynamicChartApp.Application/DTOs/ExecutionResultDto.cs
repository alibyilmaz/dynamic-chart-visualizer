using System.Collections.Generic;

namespace DynamicChartApp.Application.DTOs
{
    /// <summary>
    /// Represents the result of executing a database object (columns and rows).
    /// </summary>
    public class ExecutionResultDto
    {
        /// <summary>List of column names in the result set.</summary>
        public List<string> Columns { get; set; } = new();

        /// <summary>List of rows, each as a dictionary of column name to value.</summary>
        public List<Dictionary<string, object>> Rows { get; set; } = new();
    }
}