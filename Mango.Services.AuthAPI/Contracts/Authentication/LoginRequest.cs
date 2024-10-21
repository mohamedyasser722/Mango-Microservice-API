namespace Mango.Services.AuthAPI.Contracts.Authentication;
public record LoginRequest(

    string UserName,
    string Password

);