namespace DynamicChartApp.Domain.Models;

public class ExecutionResult
{
    public List<string> Columns { get; set; } = new();
    public List<Dictionary<string, object>> Rows { get; set; } = new();
}
