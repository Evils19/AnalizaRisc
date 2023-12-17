using System.Security.Claims;
using Analiza_Risc.Models;
using Analiza_Risc.Models.ViewModels;
using Analiza_Risc.Response;

namespace Analiza_Risc.Services;

public interface IAcauntService
{
    public Task<IBaseResponse<ClaimsIdentity>> Register(RegisterModels registerModels);
    public Task<IBaseResponse<ClaimsIdentity>> Login(LoginModels loginModels);
}