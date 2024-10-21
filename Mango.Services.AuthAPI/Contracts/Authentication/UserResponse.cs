namespace Mango.Services.AuthAPI.Contracts.Authentication;

public record UserResponse(
    string Id,
    string Name,
    string Email,
    string PhoneNumber

);