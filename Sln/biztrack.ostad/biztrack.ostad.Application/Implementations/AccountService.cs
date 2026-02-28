using Microsoft.AspNetCore.Identity;
using biztrack.ostad.Application.Common;
using biztrack.ostad.Contract.Request;
using biztrack.ostad.Domain;
using biztrack.ostad.Application.Interfaces;

namespace biztrack.ostad.Application.Implementations;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<ApplicationUser>> RegisterAsync(RegisterModel model, CancellationToken cancellationToken = default)
    {
        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
            return Result<ApplicationUser>.FailResult("A user with this email already exists.");

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            return Result<ApplicationUser>.FailResult(string.Join(" ", result.Errors.Select(e => e.Description)));

        return Result<ApplicationUser>.SuccessResult(user);
    }
}
