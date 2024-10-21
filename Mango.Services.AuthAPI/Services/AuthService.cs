
using Mango.Services.AuthAPI.Abstraction.Errors;
using Mango.Services.AuthAPI.Extensions;
using Mango.Services.AuthAPI.Helpers;
using Mango.Services.AuthAPI.Models;
using Mapster;
using System.Linq.Dynamic.Core.Tokenizer;

namespace Mango.Services.AuthAPI.Services;


public class AuthService(
    UserManager<ApplicationUser> userManager,
    AppDbContext db,
    RoleManager<IdentityRole> roleManager,
    IJwtTokenGenerator jwtTokenGenerator,
    IUserService userService) : IAuthService
{
    private readonly AppDbContext _db = db;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
    private readonly IUserService _userService = userService;

    public async Task<Result<LoginResponse>> Login(LoginRequest loginRequest, CancellationToken cancellationToken = default)
    {
        var validationResult = FluentValidationHelper.Validate(new LoginRequestValidation(), loginRequest);
        if (!validationResult.IsValid)
            return Result.Failure<LoginResponse>(Error.FromValidationResult(validationResult));

        var user = await _userManager.FindByEmailAsync(loginRequest.UserName);
        if (user == null || !(await _userManager.CheckPasswordAsync(user, loginRequest.Password)))
            return Result.Failure<LoginResponse>(AuthErrors.invalidEmailOrPassword);

        return await GenerateAuthResponseAsync(user, cancellationToken);
    }
    public async Task<Result<UserResponse>> RegisterUser(RegisterRequest registerRequest, CancellationToken cancellationToken = default)
    {

        var validationResult = FluentValidationHelper.Validate(new RegisterRequestValidator(), registerRequest);
        if (!validationResult.IsValid)
            return Result.Failure<UserResponse>(Error.FromValidationResult(validationResult));


        var user = new ApplicationUser()
        {
            UserName = registerRequest.Email,
            Email = registerRequest.Email,
            Name = registerRequest.Name,
            PhoneNumber = registerRequest.PhoneNumber,
        };

        var result = await _userManager.CreateAsync(user, registerRequest.Password);
        if (!result.Succeeded)
            return Result.Failure<UserResponse>(Error.FromIdentityResult(result));

        var userToReturn = await _db.Users.FirstOrDefaultAsync(u => u.Email == registerRequest.Email);
        var userDto = new UserResponse
        (
            userToReturn.Id,
            userToReturn.Name,
            userToReturn.Email,
            userToReturn.PhoneNumber
        );

        return Result.Success(userDto);
    }
    public async Task<Result<LoginResponse>> Refresh(string token, string refreshToken, CancellationToken cancellationToken)
    {
        var userId = _jwtTokenGenerator.ValidateToken(token);
        if (userId == null)
            return Result.Failure<LoginResponse>(AuthErrors.invalidToken);

        // find user by id
        var user = await _userManager.FindByIdWithTokensAsync(userId, cancellationToken);

        if (user == null)
            return Result.Failure<LoginResponse>(AuthErrors.userNotFound);
        if(user.LockoutEnd >  DateTime.UtcNow)
            return Result.Failure<LoginResponse>(AuthErrors.userLockedOut);

        // find refresh token
        var storedRefreshToken = user.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken && rt.IsActive);
        // check if the refresh token is valid
        if (storedRefreshToken == null)
            return Result.Failure<LoginResponse>(AuthErrors.invalidRefreshToken);
        // Invalidate the old refreshToken
        storedRefreshToken.Revoked = DateTime.UtcNow;

        
        return await GenerateAuthResponseAsync(user, cancellationToken);

    }
    public async Task<Result> AssignRole(AssignRoleRequest assignRoleRequest, CancellationToken cancellationToken = default)
    {

        var user = await _userManager.FindByEmailAsync(assignRoleRequest.Email);
        if (user == null)
            return Result.Failure(AuthErrors.userNotFound);

        // check if role exists & if not create role
        if (!await _roleManager.RoleExistsAsync(assignRoleRequest.Role))
            await _roleManager.CreateAsync(new IdentityRole(assignRoleRequest.Role));
        

        await _userManager.AddToRoleAsync(user, assignRoleRequest.Role);

        return Result.Success();
    }






    









    private async Task<Result<LoginResponse>> GenerateAuthResponseAsync(ApplicationUser user , CancellationToken cancellationToken = default)
    {
        // Get user roles
        var roles = await _userManager.GetRolesAsync(user);
        // Generate new JWT token and refresh token
        var (newToken, expiresIn) = _jwtTokenGenerator.GenerateToken(user, roles);
        var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        // Save refresh token to DB (omitted for brevity)
        user.RefreshTokens.Add(newRefreshToken);
        await _userManager.UpdateAsync(user);

        return Result.Success(new LoginResponse
        {
            User = new UserResponse(user.Id, user.Name, user.Email, user.PhoneNumber),
            Token = newToken,
            ExpiresIn = expiresIn,
            RefreshToken = newRefreshToken.Token,
            RefreshTokenExpiration = newRefreshToken.Expires
        });

    }

}




