using Analiza_Risc.Models;
using Analiza_Risc.Services;
using Microsoft.AspNetCore.Mvc;

namespace Analiza_Risc.Controllers;

public class AnalizController : Controller
{
    private readonly IServiceCRUD _serviceCrud;

    public AnalizController(IServiceCRUD serviceCrud)
    {
        _serviceCrud = serviceCrud;
    }
    
    
    public IActionResult ADD(AddCompanie addcompanie, AddActiveImobilizate activeImobilizate,
        AddActiveCirculante activeCirculante, AddDatorii addDatorii, AddCapitaluri addCapitaluri){

        _serviceCrud.InregisrCompanie(addcompanie,activeImobilizate,activeCirculante,addDatorii,addCapitaluri);

        return RedirectToAction("Index","Home");

    }



}