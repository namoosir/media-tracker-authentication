using MediaTrackerAuthenticationService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using MediaTrackerAuthenticationService.Services.AuthService;
using System.Diagnostics.Tracing;

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

    [HttpGet("sign/google")]
    public ActionResult<ServiceResponse<string>> Get()
    {
        var response = _authService.GetGoogle();
        Response.Cookies.Append("SessionToken", "sdfdsfds", new CookieOptions
        {
            IsEssential=true,
            Secure=false,
            Domain="http://127.0.0.1",
            SameSite = SameSiteMode.None
            
        });
        return Ok(response);
    }

    [HttpGet("sign/redirect/google")]
    public async Task<ActionResult> GetRedirectGoogle(
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

        Response.Cookies.Append("SessionToken", "sdfdsfds", new CookieOptions
        {
            HttpOnly = true, // The cookie is only accessible via HTTP requests
            Secure = true, // Ensure that the cookie is only sent over HTTPS
            SameSite = SameSiteMode.None, // Adjust this based on your application's requirements
        });

        return Redirect(response.Data);
    }

    [HttpGet("refreshtoken/{userId}")]
    public async Task<ActionResult> RefreshToken(int userId)
    {
        var response = await _authService.RefreshSession(userId);
        return Ok(response);
    }

    [HttpGet("logout/{userId}")]
    public async Task<ActionResult> Logout(int userId)
    {
        var response = await _authService.LogoutSession(userId);
        return Ok(response);
    }
}
