using DynamicChartApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicChartApp.Application.ResponseModel
{
    public class ResponseDto<T>
    {
        public string Status { get; set; } = "success";
        public string Message { get; set; } = "Data retrieved successfully.";
        public T Data { get; set; }
        public ExecutionMetaDto Execution { get; set; }
    }
    
    public class ResponseDto : ResponseDto<ExecutionResultDto>
    {
    }
}
