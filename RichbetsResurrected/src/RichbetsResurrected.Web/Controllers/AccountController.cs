using System.Net;
using System.Security.Claims;
using AspNet.Security.OAuth.Discord;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RichbetsResurrected.Infrastructure.Identity;
using RichbetsResurrected.Infrastructure.Identity.Models;

namespace RichbetsResurrected.Web.Controllers;

public class AccountController : Controller
{
    private readonly AccountRepository  _accountRepository;
    public AccountController(AccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    // GET
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
        // Creating a new Discord authentication properties instance (it prevents LoginProvider from being null)
        return _accountRepository.ChallengeResultAsync(DiscordAuthenticationDefaults.AuthenticationScheme, Url.Action("signin_discord", "Account", new { returnUrl }));
    }

    [Route("/signin-discord")]
    public async Task<IActionResult> signin_discord(string? returnUrl = null)
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
            return LocalRedirect(returnUrl);
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
        return RedirectToAction("Index", "Error", new { statusCode = (int)HttpStatusCode.Unauthorized });
    }
}