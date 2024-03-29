﻿using System.Security.Claims;
using AspNet.Security.OAuth.Discord;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RichbetsResurrected.Entities.DatabaseEntities.Identity.Models;
using RichbetsResurrected.Interfaces.Identity;
using RichbetsResurrected.Utilities.Constants;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace RichbetsResurrected.Identity.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

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
            UserName = info.Principal.FindFirstValue(ClaimTypes.Name), Email = info.Principal.FindFirstValue(ClaimTypes.Email)
        };
        var result = await _userManager.CreateAsync(user);
        return (result, user);
    }

    public async Task<IdentityResult> AddExternalLoginToUserAsync(AppUser user, ExternalLoginInfo info)
    {
        var result = await _userManager.AddLoginAsync(user, info);
        return result;
    }

    public async Task UpdateDiscordClaimsAsync(ExternalLoginInfo info)
    {
        var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
        var userClaims = await _userManager.GetClaimsAsync(user);

        var userClaim = userClaims.FirstOrDefault(c => c.Type == OAuthConstants.DiscordId);
        var avatarClaim = userClaims.FirstOrDefault(c => c.Type == DiscordAuthenticationConstants.Claims.AvatarHash);

        var refreshSignIn = false;

        if (info.Principal.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
        {
            var externalClaim = info.Principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);

            if (userClaim == null)
            {
                await _userManager.AddClaimAsync(user, new Claim(OAuthConstants.DiscordId, externalClaim.Value));
                refreshSignIn = true;

            }
            else if (userClaim.Value != externalClaim.Value)
            {
                await _userManager.RemoveClaimAsync(user, userClaim);
                await _userManager.AddClaimAsync(user, new Claim(OAuthConstants.DiscordId, externalClaim.Value));
                refreshSignIn = true;
            }

        }
        else if (userClaim == null)
        {
            await _userManager.AddClaimAsync(user, new Claim(OAuthConstants.DiscordId, "0"));
            refreshSignIn = true;
        }


        if (info.Principal.HasClaim(c => c.Type == DiscordAuthenticationConstants.Claims.AvatarHash))
        {
            var externalClaim = info.Principal.FindFirst(c => c.Type == DiscordAuthenticationConstants.Claims.AvatarHash);

            if (avatarClaim == null)
            {
                await _userManager.AddClaimAsync(user, new Claim(DiscordAuthenticationConstants.Claims.AvatarHash, externalClaim.Value));
                refreshSignIn = true;

            }
            else if (avatarClaim.Value != externalClaim.Value)
            {
                await _userManager.RemoveClaimAsync(user, avatarClaim);
                await _userManager.AddClaimAsync(user, new Claim(avatarClaim.Type, externalClaim.Value));
                refreshSignIn = true;
            }
        }
        else if (avatarClaim == null)
        {
            await _userManager.AddClaimAsync(user, new Claim(avatarClaim.Type, "0"));
            refreshSignIn = true;
        }

        if (user.UserName != info.Principal.FindFirstValue(ClaimTypes.Name))
        {
            var usernameNew = info.Principal.FindFirstValue(ClaimTypes.Name);
            await _userManager.SetUserNameAsync(user, usernameNew);
            await _userManager.UpdateNormalizedUserNameAsync(user);
            refreshSignIn = true;
        }

        if (refreshSignIn) await _signInManager.RefreshSignInAsync(user);
    }

    public async Task UpdateRichbetUserAsync(AppUser user, ExternalLoginInfo info)
    {
    }
    public string GetDiscordAvatarUrl(ClaimsPrincipal user)
    {
        var discordId = user.Claims.FirstOrDefault(c => c.Type == OAuthConstants.DiscordId).Value;
        var avatarHash = user.FindFirst(c => c.Type == DiscordAuthenticationConstants.Claims.AvatarHash)?.Value;
        var avatarFileName = avatarHash.StartsWith("a_") ? $"{avatarHash}.gif" : $"{avatarHash}.png";

        var avatarUrl = $"https://cdn.discordapp.com/avatars/{discordId}/{avatarFileName}";
        return avatarUrl;
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