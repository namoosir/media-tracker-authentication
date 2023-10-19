using MediaTrackerAuthenticationService.Models.Utils;
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
        // Response.Cookies.Append("SessionToken", "sdfdsfds", new CookieOptions
        // {
        //     IsEssential=true,
        //     Path="/"
        // });


        // // // Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:5173");
        // // // Response.Headers.Add("Test", "test");
        // // // return Ok();
        Response.Headers.Add("Access-Control-Allow-Origin", "*");
        // Response.Headers.Add("Access-Control-Allow-Credentials", "true");
        
        // // Response.Cookies.Append("MyCookie", "CookieValue");
        // return Redirect("https://www.google.com/");
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
