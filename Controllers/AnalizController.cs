using System.Security.Claims;
using Analiza_Risc.Data;
using Analiza_Risc.Models;
using Analiza_Risc.Response;
using Analiza_Risc.Services;
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


    [HttpPost]

    public IActionResult ADD(ClaimsIdentity response, AddActiveImobilizate activeImobilizate,
        AddActiveCirculante activeCirculante, AddDatorii addDatorii, AddCapitaluri addCapitaluri)
    {

        response = (ClaimsIdentity)User.Identity;
        _serviceCrud.InregisrCompanie(response, activeImobilizate, activeCirculante, addDatorii, addCapitaluri);

        return RedirectToAction("Index", "Home");

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





}


