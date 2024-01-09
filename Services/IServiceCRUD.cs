using System.Security.Claims;
using Analiza_Risc.Models;
using Analiza_Risc.Response;

namespace Analiza_Risc.Services;

public interface IServiceCRUD
{

    public Task<IBaseResponse<Companie>> Update(ClaimsIdentity respons, AddActiveImobilizate activeImobilizate,
        AddActiveCirculante activeCirculante, AddDatorii addDatorii, AddCapitaluri addCapitaluri); 

    Task<InfoData> GetInfo(ClaimsIdentity response);
    Task<InfoData> GetCompani(ClaimsIdentity response); 
    void Delete(ClaimsIdentity response);
}

