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
        var result = await _dataService.ExecuteAndLogAsync(requestDto, HttpContext.Request.Path);
        if (result.Status == "Error")
            return StatusCode(500, result);
        return Ok(result);
    }

    [HttpPost("objects")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> ListObjects([FromBody] ListObjectsRequestDto requestDto)
    {
        var result = await _dataService.ListObjectsAndLogAsync(requestDto.Host, requestDto.Database, requestDto.Username, requestDto.Password, requestDto.Type, HttpContext.Request.Path);
        if (result.Status == "Error")
            return StatusCode(500, result);
        return Ok(result);
    }
}
