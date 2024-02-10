namespace Auction.Application.Abstractions;

public interface IPasswordHasher
{
    string Hash(string password);
}