using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Auction.Infrastructure.Implementations;

public class JsonWebTokenService(
    JsonWebTokenHandler tokenHandler,
    [FromKeyedServices("jwt")] SigningCredentials signingCredentials
    )
{
    public string Create(Dictionary<string, object>? claims = null)
    {
        var issuedAt = DateTime.UtcNow;
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            IssuedAt = issuedAt,
            Issuer = "auction",
            Audience = "auction",
            SigningCredentials = signingCredentials,
            Claims = claims
        };

        return tokenHandler.CreateToken(tokenDescriptor);
    }
}