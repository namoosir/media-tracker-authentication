using MediaTrackerAuthenticationService.Dtos.PlatformConnection;
using MediaTrackerAuthenticationService.Models.Utils;
using MediaTrackerAuthenticationService.Services.PlatformConnectionService;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace MediaTrackerAuthenticationService.Controllers;

[ApiController]
[Route("[controller]")]
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

    // [HttpPost]
    // public async Task<ActionResult<ServiceResponse<GetPlatformConnectionDto>>> Post(
    //     AddPlatformConnectionDto newPlatformConnection
    // )
    // {
    //     return Ok(await _platformConnectionService.AddPlatformConnection(newPlatformConnection));
    // }

    // [HttpPut]
    // public async Task<ActionResult<ServiceResponse<GetPlatformConnectionDto>>> Put(
    //     UpdatePlatformConnectionDto updatedPlatformConnection
    // )
    // {
    //     var response = await _platformConnectionService.UpdatePlatformConnection(
    //         updatedPlatformConnection
    //     );

    //     if (response.Data is null)
    //     {
    //         return NotFound(response);
    //     }
    //     return Ok(response);
    // }

    [HttpGet("action/request/youtube")]
    public ActionResult<ServiceResponse<string>> GetYoutube()
    {
        var response = _platformConnectionService.GetYoutube();
        return Ok(response);
    }

    [HttpGet("action/redirect/youtube")]
    public async Task<ActionResult> GetRedirectYoutube(
        [FromQuery] IDictionary<string, string> queryParameters
    )
    {
        string? code = queryParameters.TryGetValue("code", out string? result) ? result : null;
        string? error = queryParameters.TryGetValue("error", out string? result2) ? result2 : null;

        var response = await _platformConnectionService.GetRedirectYoutube(code, error);

        Console.WriteLine(response.Message);

        if (response.Data is null)
        {
            return StatusCode(500, response); //TODO: change to correct status code and maybe add a better message
        }

        return Redirect(response.Data);
    }
}
