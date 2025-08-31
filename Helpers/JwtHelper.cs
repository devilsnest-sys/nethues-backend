using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
namespace ArithmeticChat.Helpers;
public sealed class JwtHelper
{
    private readonly string _key;
    private readonly string _issuer;
    private readonly string _audience;
    public JwtHelper(IConfiguration cfg)
    {
        _key = cfg["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key");
        _issuer = cfg["Jwt:Issuer"] ?? "ArithmeticChat";
        _audience = cfg["Jwt:Audience"] ?? "ArithmeticChatClient";
    }
    public string CreateToken(int userId, string username, int expiresHours = 8)
    {
        var claims = new List<Claim> {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, username)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(_issuer, _audience, claims, expires: DateTime.UtcNow.AddHours(expiresHours), signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
