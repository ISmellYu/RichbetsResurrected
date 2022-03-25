﻿using AspNet.Security.OAuth.Discord;
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
    
    public Task ExternalLogin()
    {
        var returnUrl = "";
        if (ViewData["ReturnUrl"] == null)
        {
            returnUrl = Url.Action("Index", "Home");
        }
        var properties = new AuthenticationProperties { RedirectUri = returnUrl };
        return HttpContext.ChallengeAsync(DiscordAuthenticationDefaults.AuthenticationScheme, properties);
    }
    
    
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync().ConfigureAwait(false);
        return RedirectToAction("Index", "Home");
    }
    
    public async Task<IActionResult> AccessDenied()
    {
        return RedirectToAction("Index", "Home");
    }
}