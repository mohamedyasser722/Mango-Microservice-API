namespace Mango.Services.AuthAPI.Extensions;

public static class UserManagerExtensions
{
    public static async Task<ApplicationUser?> FindByIdWithTokensAsync(this UserManager<ApplicationUser> userManager, string userId, CancellationToken cancellationToken = default)
    {
        return await userManager.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }
}
