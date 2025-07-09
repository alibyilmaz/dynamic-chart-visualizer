namespace DynamicChartApp.Domain.Models;

public class LogEntry
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Username { get; set; } = "";
    public string Path { get; set; } = "";
    public string RequestBody { get; set; } = "";
    public string ResponseBody { get; set; } = "";
    public int StatusCode { get; set; }
}
