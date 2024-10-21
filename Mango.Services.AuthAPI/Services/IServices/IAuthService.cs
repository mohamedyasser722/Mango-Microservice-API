namespace Mango.Services.AuthAPI.Services.IServices;

public interface IAuthService
{
    // Register
    Task<Result<UserResponse>> RegisterUser(RegisterRequest registerRequest, CancellationToken cancellationToken = default);
    // Login
    Task<Result<LoginResponse>> Login(LoginRequest loginRequest, CancellationToken cancellationToken = default);
    // Refresh
    Task<Result<LoginResponse>> Refresh(string token, string refreshToken, CancellationToken cancellationToken = default);
    // Assign Roles
    Task<Result> AssignRole(AssignRoleRequest assignRoleRequest, CancellationToken cancellationToken = default);
}
