using System.Security.Claims;
using Analiza_Risc.Models;
using Analiza_Risc.Response;

namespace Analiza_Risc.Services;

public interface IServiceCRUD
{

    void InregisrCompanie(ClaimsIdentity respons, AddActiveImobilizate activeImobilizate,
        AddActiveCirculante activeCirculante, AddDatorii addDatorii, AddCapitaluri addCapitaluri); 
    Task<InfoData> GetInfo(ClaimsIdentity response);
}

