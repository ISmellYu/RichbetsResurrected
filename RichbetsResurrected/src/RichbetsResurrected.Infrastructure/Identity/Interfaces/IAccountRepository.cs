using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RichbetsResurrected.Infrastructure.Identity.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace RichbetsResurrected.Infrastructure.Identity.Interfaces;

public interface IAccountRepository
{
    Task<ExternalLoginInfo?> GetExternalLoginInfoAsync();
    Task<SignInResult> ExternalLoginSignInAsync(ExternalLoginInfo info);
    Task<(IdentityResult, AppUser)> CreateUserFromExternalLoginAsync(ExternalLoginInfo info);
    Task<IdentityResult> AddExternalLoginToUserAsync(AppUser user, ExternalLoginInfo info);
    Task<IActionResult> ChallengeResultAsync(string providerSchemaName, string? redirectUrl = null);
    Task LoginAsync(AppUser user);
    Task LogoutAsync();
}