﻿using DynamicChartApp.Domain.Enums;

namespace DynamicChartApp.Domain.Models;

public class ExecutionRequest
{
    public string Host { get; set; } = "";
    public string Database { get; set; } = "";
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string ObjectName { get; set; } = "";
    public DataObjectType ObjectType { get; set; }
    public Dictionary<string, object>? Parameters { get; set; }
}
