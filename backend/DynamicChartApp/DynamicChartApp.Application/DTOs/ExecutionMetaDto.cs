using System;

namespace DynamicChartApp.Application.DTOs
{
    /// <summary>
    /// Metadata about the execution of a database object (timing info).
    /// </summary>
    public class ExecutionMetaDto
    {
        /// <summary>Execution duration in milliseconds.</summary>
        public double DurationMs { get; set; }

        /// <summary>Timestamp when execution started.</summary>
        public DateTime StartedAt { get; set; }

        /// <summary>Timestamp when execution finished.</summary>
        public DateTime FinishedAt { get; set; }
    }
}