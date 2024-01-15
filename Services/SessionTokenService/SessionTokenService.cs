using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using MediaTrackerAuthenticationService.utils;
using System.Security.Claims;

namespace MediaTrackerAuthenticationService.Services.SessionTokenService;
public class SessionTokenService : ISessionTokenService
{
    private readonly IConfiguration _configuration;

    public SessionTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(int userId)
    {
        var secret = _configuration["JwtSessionSecretSigningKey"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Site:Name"],
            audience: _configuration["Client:Web"],
            claims: new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            },
            expires: DateTime.Now.AddHours(1), // Token expires in 1 hour
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}





