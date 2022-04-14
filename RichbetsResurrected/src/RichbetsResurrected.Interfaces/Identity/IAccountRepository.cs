using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RichbetsResurrected.Entities.Identity.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace RichbetsResurrected.Interfaces.Identity;

public interface IAccountRepository
{
    Task<ExternalLoginInfo?> GetExternalLoginInfoAsync();
    Task<SignInResult> ExternalLoginSignInAsync(ExternalLoginInfo info);
    Task<(IdentityResult, AppUser)> CreateUserFromExternalLoginAsync(ExternalLoginInfo info);
    Task<IdentityResult> AddExternalLoginToUserAsync(AppUser user, ExternalLoginInfo info);
    Task UpdateDiscordClaimsAsync(ExternalLoginInfo info);
    Task UpdateRichbetUserAsync(AppUser user, ExternalLoginInfo info);
    string GetDiscordAvatarUrlAsync(ClaimsPrincipal user);
    Task<IActionResult> ChallengeResultAsync(string providerSchemaName, string? redirectUrl = null);
    Task LoginAsync(AppUser user);
    Task LogoutAsync();
}