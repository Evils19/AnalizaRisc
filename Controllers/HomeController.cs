using System.Diagnostics;
using System.Security.Claims;
using Analiza_Risc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Analiza_Risc.Controllers;
[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IServiceCRUD _serviceCrud;
  
    public HomeController(IServiceCRUD serviceCrud)
    {
        _serviceCrud = serviceCrud;
    }

    public async Task<IActionResult> Index()
    {
        ClaimsIdentity response = (ClaimsIdentity)User.Identity;
       
                var infoData = await _serviceCrud.GetInfo(response);
                ViewBag.ActiveImobilizate = infoData.ActiveImobilizate;
                ViewBag.ActiveCirculante = infoData.ActiveCirculante;
                ViewBag.Datorii = infoData.Datorii;
                ViewBag.Capitaluri = infoData.Capitaluri;
                ViewBag.RationFinanciar = infoData.RationFinanciar;
                ViewBag.IndicatorR = infoData.IndicatorR;
                var feedback = HttpContext.Session.GetString("Feedback");
                ViewBag.Rsp = feedback;
                return View();
 
    }


    public async Task<IActionResult> Update1()
    {
        var identity = (ClaimsIdentity)User.Identity;
        var infoData = await _serviceCrud.GetCompani(identity);
        ViewBag.Companie= infoData.Companie;
        ViewBag.ActiveImobilizate = infoData.ActiveImobilizate;
        ViewBag.ActiveCirculante = infoData.ActiveCirculante;
        ViewBag.Datorii = infoData.Datorii;
        ViewBag.Capitaluri = infoData.Capitaluri;
        return View();
    }


    public IActionResult DeleteCont()
    {
        return View();
    }


}