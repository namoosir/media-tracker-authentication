using MediaTrackerAuthenticationService.Dtos.Auth;
using MediaTrackerAuthenticationService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using MediaTrackerAuthenticationService.Services.AuthService;

namespace MediaTrackerAuthenticationService.Controllers;

[ApiController]
[Route("[controller]")]
public class Auth : ControllerBase
{
    private readonly IAuthService _authService;

    public Auth(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("sign/google")]
    public ActionResult<ServiceResponse<string>> Get()
    {
        var response = _authService.GetGoogle();
        return Ok(response);
    }

    [HttpGet("sign/redirect/google")]
    public async Task<ActionResult> GetRedirectYoutube(
        [FromQuery] IDictionary<string, string> queryParameters
    )
    {
        string? code = queryParameters.TryGetValue("code", out string? result) ? result : null;
        string? error = queryParameters.TryGetValue("error", out string? result2) ? result2 : null;

        var response = await _authService.GetRedirectGoogle(code, error);

        Console.WriteLine(response.Message);

        if (response.Data is null)
        {
            return StatusCode(500, response); //TODO: change to correct status code and maybe add a better message
        }

        return Redirect(response.Data);
    }
}
