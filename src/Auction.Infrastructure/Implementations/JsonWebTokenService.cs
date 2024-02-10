using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Auction.Infrastructure.Implementations;

public class JsonWebTokenService(
    JsonWebTokenHandler tokenHandler,
    SigningCredentials signingCredentials
    )
{
    public string Create()
    {
        var issuedAt = DateTime.UtcNow;
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            IssuedAt = issuedAt,
            SigningCredentials = signingCredentials
        };

        return tokenHandler.CreateToken(tokenDescriptor);
    }
}