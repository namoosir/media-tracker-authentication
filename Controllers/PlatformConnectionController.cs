using MediaTrackerAuthenticationService.Dtos.PlatformConnection;
using MediaTrackerAuthenticationService.Models;
using MediaTrackerAuthenticationService.Services.PlatformConnectionService;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet("{UserId}")]
    public async Task<ActionResult<ServiceResponse<GetPlatformConnectionDto>>> Get(int UserId)
    {
        return Ok(await _platformConnectionService.GetPlatformConnectionByUserId(UserId));
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

    [HttpGet("youtube")]
    public RedirectResult GetYoutube()
    {
        return Redirect("https://www.youtube.com/");
    }
}
