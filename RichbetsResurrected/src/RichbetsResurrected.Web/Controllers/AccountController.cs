using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RichbetsResurrected.Infrastructure.Data.Identity.Models;

namespace RichbetsResurrected.Web.Controllers;

public class AccountController : Controller
{
    
    private readonly SignInManager<AppUser> _signInManager;
    public AccountController(SignInManager<AppUser> signInManager)
    {
        _signInManager = signInManager;
    }

    // GET
    [HttpGet]
    public async Task<IActionResult> Login(string? returnUrl = null)
    {
        return View();
    }

}