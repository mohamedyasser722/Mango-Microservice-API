namespace Mango.Services.AuthAPI.Contracts.Authentication;

public record AssignRoleRequest
(
    string Email,
    string Role
);
