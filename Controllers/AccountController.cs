using Analiza_Risc.Models.ViewModels;
using Analiza_Risc.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Analiza_Risc.Models.Enum;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace Analiza_Risc.Controllers;

public class AccountController : Controller
{

    private readonly IAcauntService _acauntService;

    public AccountController(IAcauntService acauntService)
    {
        _acauntService = acauntService;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    
    public  async Task<IActionResult> Register(RegisterModels registerModels)
    {
        if (ModelState.IsValid)
        {
            var response = await _acauntService.Register(registerModels);
            if (response.StatusCode == StatusCodeUser.ok)
            {
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(response.Data));

                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", response.Description);
        }
        return View(registerModels);
        
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModels model)
    {
        if (ModelState.IsValid)
        {
            var response = await _acauntService.Login(model);
            if (response.StatusCode == StatusCodeUser.ok)
            {
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(response.Data));

                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", response.Description);
        }
        return View(model);
    }

    
    
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}
    



   
