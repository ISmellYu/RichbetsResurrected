using System.Net;
using System.Security.Claims;
using AspNet.Security.OAuth.Discord;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RichbetsResurrected.Infrastructure.Data.Identity.Models;

namespace RichbetsResurrected.Web.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
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
    
    public IActionResult ExternalLogin(string returnUrl = null)
    {
        // Creating a new Discord authentication properties instance (it prevents LoginProvider from being null)
        var prop = _signInManager.ConfigureExternalAuthenticationProperties(
            DiscordAuthenticationDefaults.AuthenticationScheme, Url.Action("signin_discord", "Account", new {
                returnUrl}));
        return new ChallengeResult(DiscordAuthenticationDefaults.AuthenticationScheme, prop);
    }

    [Route("/signin-discord")]
    public async Task<IActionResult> signin_discord(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            return RedirectToAction("Login");
        }
        
        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false).ConfigureAwait(false);
        if (result.Succeeded)
        {
            return LocalRedirect(returnUrl);
        }
        else
        {
            var user = new AppUser
            {
                UserName = info.Principal.FindFirstValue(ClaimTypes.Name),
                Email = info.Principal.FindFirstValue(ClaimTypes.Email)
            };

            var createResult = await _userManager.CreateAsync(user);
            if (createResult.Succeeded)
            {
                createResult = await _userManager.AddLoginAsync(user, info);
                if (createResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return LocalRedirect(returnUrl);
                }
            }
            return RedirectToAction("Login");
        }
        return RedirectToAction("Index", "Home");
    }
    
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync().ConfigureAwait(false);
        return RedirectToAction("Login", "Account");
    }
    
    public async Task<IActionResult> AccessDenied()
    {
        return RedirectToAction("Index", "Error", new { statusCode = (int)HttpStatusCode.Unauthorized });
    }
}