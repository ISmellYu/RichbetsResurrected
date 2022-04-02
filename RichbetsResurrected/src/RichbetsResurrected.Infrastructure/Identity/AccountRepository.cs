using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RichbetsResurrected.Infrastructure.Identity.Interfaces;
using RichbetsResurrected.Infrastructure.Identity.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace RichbetsResurrected.Infrastructure.Identity;

public class AccountRepository : IAccountRepository
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    
    public AccountRepository(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    public async Task<ExternalLoginInfo?> GetExternalLoginInfoAsync()
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        return info;
    }
    
    public async Task<SignInResult> ExternalLoginSignInAsync(ExternalLoginInfo info)
    {
        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
        return result;
    }
    
    public async Task<(IdentityResult, AppUser)> CreateUserFromExternalLoginAsync(ExternalLoginInfo info)
    {
        var user = new AppUser
        {
            UserName = info.Principal.FindFirstValue(ClaimTypes.Name),
            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
        };
        var result = await _userManager.CreateAsync(user);
        return (result, user);
    }
    
    public async Task<IdentityResult> AddExternalLoginToUserAsync(AppUser user, ExternalLoginInfo info)
    {
        var result = await _userManager.AddLoginAsync(user, info);
        return result;
    }

    public async Task<IActionResult> ChallengeResultAsync(string providerSchemaName, string? redirectUrl = null)
    {
        var prop = _signInManager.ConfigureExternalAuthenticationProperties(
            providerSchemaName, redirectUrl);
        return new ChallengeResult(providerSchemaName, prop);
    }
    
    public Task LoginAsync(AppUser user)
    {
        return _signInManager.SignInAsync(user, false);
    }

    public Task LogoutAsync()
    {
        return _signInManager.SignOutAsync();
    }

}