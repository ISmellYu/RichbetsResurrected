using System.Net;
using System.Security.Claims;
using AspNet.Security.OAuth.Discord;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RichbetsResurrected.Interfaces.DAL;
using RichbetsResurrected.Interfaces.Identity;

namespace RichbetsResurrected.Web.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class AccountController : Controller
{
    private readonly IAccountRepository _accountRepository;
    private readonly IRichbetRepository _richbetRepository;

    public AccountController(IAccountRepository accountRepository, IRichbetRepository richbetRepository)
    {
        _accountRepository = accountRepository;
        _richbetRepository = richbetRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Login(string returnUrl = null)
    {
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme).ConfigureAwait(false);
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme).ConfigureAwait(false);
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    public Task<IActionResult> ExternalLogin(string returnUrl = null)
    {
        return _accountRepository.ChallengeResultAsync(DiscordAuthenticationDefaults.AuthenticationScheme,
            Url.Action("Signin_discord", "Account", new {returnUrl}));
    }

    [Route("/signin-discord")]
    public async Task<IActionResult> Signin_discord(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        var info = await _accountRepository.GetExternalLoginInfoAsync();
        if (info == null)
        {
            return RedirectToAction("Login");
        }

        var result = await _accountRepository.ExternalLoginSignInAsync(info);
        if (result.Succeeded)
        {
            await _accountRepository.UpdateDiscordClaimsAsync(info);
            return LocalRedirect(returnUrl);
        }

        if (!info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
        {
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login");
            }
        }

        var (createResult, user) = await _accountRepository.CreateUserFromExternalLoginAsync(info);
        if (!createResult.Succeeded)
        {
            return RedirectToAction("Login");
        }

        createResult = await _accountRepository.AddExternalLoginToUserAsync(user, info);
        if (!createResult.Succeeded)
        {
            return RedirectToAction("Login");
        }

        await _accountRepository.UpdateDiscordClaimsAsync(info);
        await _richbetRepository.CreateRichbetUserAsync(user.Id,
            info.Principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
        await _accountRepository.LoginAsync(user);
        return LocalRedirect(returnUrl);
    }

    public async Task<IActionResult> Logout()
    {
        await _accountRepository.LogoutAsync();
        return RedirectToAction("Login", "Account");
    }

    public async Task<IActionResult> AccessDenied()
    {
        return RedirectToAction("Index", "Error", new {statusCode = (int)HttpStatusCode.Unauthorized});
    }
}
