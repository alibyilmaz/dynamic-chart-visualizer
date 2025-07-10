using DynamicChartApp.Application.DTOs;

namespace DynamicChartApp.Application.ResponseModel
{
    public class ResponseDto<T>
    {
        public string Status { get; set; } = "success";
        public string Message { get; set; } = "Data retrieved successfully.";
        public T? Data { get; set; }
        public ExecutionMetaDto? Execution { get; set; }
    }
    
    public class ResponseDto : ResponseDto<ExecutionResultDto>
    {
    }
}
