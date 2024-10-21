namespace Mango.Services.AuthAPI.Contracts.Authentication;

public record RefreshTokenRequest
(
    string token,
    string refreshToken
);
