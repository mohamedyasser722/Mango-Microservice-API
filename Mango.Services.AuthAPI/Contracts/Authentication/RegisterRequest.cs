namespace Mango.Services.AuthAPI.Contracts.Authentication;

public record RegisterRequest(
  
    string Email,
    string Name,
    string PhoneNumber,
    string Password

);