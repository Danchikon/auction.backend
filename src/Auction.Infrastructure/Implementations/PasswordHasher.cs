using System.Security.Cryptography;
using System.Text;
using Auction.Application.Abstractions;

namespace Auction.Infrastructure.Implementations;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        var bytes = Encoding.UTF8.GetBytes(password);
        
        var hashBytes = SHA256.HashData(bytes);

        var hash = Encoding.UTF8.GetString(hashBytes);

        return hash;
    }
}