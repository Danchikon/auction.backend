namespace Auction.Domain.Common.Errors;

public enum ErrorKind
{
    Unknown,
    PermissionDenied,
    InvalidData,
    InvalidOperation,
    NotFound,
}