using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace MediaTrackerAuthenticationService.Services.SessionTokenService;
public class SessionTokenService : ISessionTokenService
{
    //     private readonly IConfiguration _configuration;

    //     public RequestUrlBuilderService(IConfiguration configuration)
    //     {
    //         _configuration = configuration;
    //     }

    // public string GenerateToken(int userId, string username)
    // {
    //     var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
    //     var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    //     var token = new JwtSecurityToken(
    //         issuer: "your_issuer",
    //         audience: "your_audience",
    //         claims: new[]
    //         {
    //             new Claim(JwtRegisteredClaimNames.Sub, username),
    //             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    //             new Claim("userId", userId.ToString())
    //         },
    //         expires: DateTime.Now.AddHours(1), // Token expires in 1 hour
    //         signingCredentials: creds
    //     );

    //     return new JwtSecurityTokenHandler().WriteToken(token);
    // }
}





