using System.Security.Claims;
using Analiza_Risc.Data;
using Analiza_Risc.Models;
using Analiza_Risc.Models.Enum;
using Analiza_Risc.Models.ViewModels;
using Analiza_Risc.Response;
using Analiza_Risc.Services.Helplers;
using Microsoft.EntityFrameworkCore;

namespace Analiza_Risc.Services;


public class AcauntService : IAcauntService
{
    private readonly AppDbContext _context;
    private readonly ILogger _logger;

    public AcauntService(AppDbContext context, ILogger<AcauntService> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<IBaseResponse<ClaimsIdentity>> Login(LoginModels loginModels)
    {
        try
        {
            var user = new Companie();
            user = await _context.Companies.FirstOrDefaultAsync(x => x.Nume_Companie == loginModels.Numele_Companiei);
            if (user==null)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = "Companie cu asa nume nu exista",
                };
            }

            if (user.parola!= HashPassword.Hash256(loginModels.parola))
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = "Parola nu este corecta",
                };
            }
            var result = Authenticate(user);

            return new BaseResponse<ClaimsIdentity>()
            {
                Data = result,
                Description = "Companie a fost logata cu succes",
                StatusCode = StatusCodeUser.ok
            };
        }
        catch (Exception e)
        {
           _logger.LogError(e,$"[Login]:{e.Message}");
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = e.Message,
                    StatusCode = StatusCodeUser.ServerError
                };
        }

    }
    
    public async Task<IBaseResponse<ClaimsIdentity>> Register(RegisterModels registerModels)
    {
        try
        {
            var user= await _context.Companies.FirstOrDefaultAsync(x => x.Nume_Companie == registerModels.Numele_Companiei);
            if (user!= null)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = "Companie cu asa nume deja a fost inregistrata adresativa la Seful Contabil al companiei sau Directorul General",
                };
                
            }

            user = new Companie()
            {
                Nume_Companie = registerModels.Numele_Companiei,
                CUI = registerModels.CUI,
                Role = Role.user,
                parola = HashPassword.Hash256(registerModels.parola),
            };
            await _context.Companies.AddAsync(user);
            await _context.SaveChangesAsync();
            var result = Authenticate(user);
            return new BaseResponse<ClaimsIdentity>()
            {
             Data = result,
             Description = "Companie a fost inregistrata cu succes",
             StatusCode = StatusCodeUser.ok
            };

        }
        catch (Exception e)
        {
           _logger.LogError(e,$"[Register]:{e.Message}");
           return new BaseResponse<ClaimsIdentity>()
           {
            Description = e.Message,
            StatusCode = StatusCodeUser.ServerError
           };
        }
        
    }

    private ClaimsIdentity Authenticate(Companie companie)
    {
        var Info = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, companie.Nume_Companie),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, companie.Role.ToString()),
            new Claim("Id_Companie", companie.Id_Companie.ToString(), ClaimValueTypes.Integer) // Specificați ClaimValueTypes.Integer pentru Id_Companie
        };
        return new ClaimsIdentity(Info, "AplicationCookie",
        ClaimsIdentity.DefaultNameClaimType,ClaimsIdentity.DefaultNameClaimType);
    }


}