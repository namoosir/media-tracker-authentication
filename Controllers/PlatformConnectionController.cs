using MediaTrackerAuthenticationService.Dtos.PlatformConnection;
using MediaTrackerAuthenticationService.Models;
using MediaTrackerAuthenticationService.Services.PlatformConnectionService;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace MediaTrackerAuthenticationService.Controllers;

[ApiController]
[Route("auth/[controller]")]
public class PlatformConnectionController : ControllerBase
{
    private readonly IPlatformConnectionService _platformConnectionService;

    public PlatformConnectionController(IPlatformConnectionService platformConnectionService)
    {
        _platformConnectionService = platformConnectionService;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<ServiceResponse<GetPlatformConnectionDto>>> Get(int userId)
    {
        var response = await _platformConnectionService.GetPlatformConnectionByUserId(userId);

        if (response.Data is null)
        {
            return NotFound(response);
        }
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<GetPlatformConnectionDto>>> Post(
        AddPlatformConnectionDto newPlatformConnection
    )
    {
        return Ok(await _platformConnectionService.AddPlatformConnection(newPlatformConnection));
    }

    [HttpPut]
    public async Task<ActionResult<ServiceResponse<GetPlatformConnectionDto>>> Put(
        UpdatePlatformConnectionDto updatedPlatformConnection
    )
    {
        var response = await _platformConnectionService.UpdatePlatformConnection(
            updatedPlatformConnection
        );

        if (response.Data is null)
        {
            return NotFound(response);
        }
        return Ok(response);
    }

    [HttpGet("request/youtube")]
    public RedirectResult GetYoutube()
    {
        var response = _platformConnectionService.GetYoutube();
        return Redirect(response.Data);
    }

    [HttpGet("redirect/youtube")]
    public async Task<RedirectResult> GetRedirectYoutube([FromQuery] IDictionary<string, string> queryParameters)
    {        
        string? code = queryParameters.TryGetValue("code", out string result) ? result : (string?)null;
        string? error = queryParameters.TryGetValue("error", out string result2) ? result2 : (string?)null;

        var response = await _platformConnectionService.GetRedirectYoutube(code, error);

        Console.WriteLine(response.Message);

        return Redirect(response.Data);
    }
}
