using System.Security.Claims;
using Analiza_Risc.Data;
using Analiza_Risc.Models;
using Analiza_Risc.Models.Enum;
using Analiza_Risc.Response;
using Analiza_Risc.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Analiza_Risc.Controllers;

[Authorize]
public class AnalizController : Controller
{
    private readonly IServiceCRUD _serviceCrud;

    
    public AnalizController(IServiceCRUD serviceCrud)
    {
        _serviceCrud = serviceCrud;
    }

    
    public async Task<IActionResult> Info()
    {

        ClaimsIdentity response = (ClaimsIdentity)User.Identity;
        
        var infoData = _serviceCrud.GetInfo(response).Result;
        
        
        ViewBag.ActiveImobilizate = infoData.ActiveImobilizate;
        ViewBag.ActiveCirculante = infoData.ActiveCirculante;
        ViewBag.Datorii = infoData.Datorii;
        ViewBag.Capitaluri = infoData.Capitaluri;
        ViewBag.RationFinanciar = infoData.RationFinanciar;
        ViewBag.IndicatorR = infoData.IndicatorR.NivelR;
        return RedirectToAction("Index", "Home");




    }



    [HttpPost]
    public async Task<IActionResult> AddUpdate1( AddActiveImobilizate activeImobilizate,
        AddActiveCirculante activeCirculante, AddDatorii addDatorii, AddCapitaluri addCapitaluri)
    {
    
        var identity = (ClaimsIdentity)User.Identity;
      var  response = await _serviceCrud.Update(identity,activeImobilizate, activeCirculante, addDatorii, addCapitaluri);
      if (response.StatusCode == StatusCodeUser.ok)
      {
          
          HttpContext.Session.SetString("Feedback", response.Description);
          return RedirectToAction("Index", "Home");
      }
      return RedirectToAction("Index", "Home");
    
    }
    
    public IActionResult Delete()
    {
        
        var identity = (ClaimsIdentity)User.Identity;
        _serviceCrud.Delete(identity);
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }

}


