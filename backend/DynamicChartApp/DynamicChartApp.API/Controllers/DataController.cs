using DynamicChartApp.Application.DTOs;
using DynamicChartApp.Application.ResponseModel;
using DynamicChartApp.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DynamicChartApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DataController : ControllerBase
{
    private readonly DataService _dataService;

    public DataController(DataService dataService)
    {
        _dataService = dataService;
    }

    [HttpPost("execute")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Execute([FromBody] ExecutionRequestDto requestDto)
    {
        try
        {
            var result = await _dataService.ExecuteAndLogAsync(requestDto, HttpContext.Request.Path);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseDto<object>
            {
                Status = "error",
                Message = ex.Message,
                Data = null
            });
        }
    }

    [HttpPost("objects")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> ListObjects([FromBody] ListObjectsRequestDto requestDto)
    {
        try
        {
            var result = await _dataService.ListObjectsAsync(requestDto.Host, requestDto.Database, requestDto.Username, requestDto.Password, requestDto.Type);
            return Ok(new ResponseDto<List<string>>
            {
                Status = "success",
                Message = "Objects listed successfully.",
                Data = result
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseDto<object>
            {
                Status = "error",
                Message = ex.Message,
                Data = null
            });
        }
    }
}
