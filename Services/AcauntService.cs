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
            var user1 = await _context.Companies.FirstOrDefaultAsync(x => x.CUI == registerModels.CUI);
            if (user1!= null)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = "Companie cu asa CUI deja a fost inregistrata adresativa la Seful Contabil al companiei sau Directorul General",
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
            
            // Salvare Bilantului setat la 0
               Active_Imobilizate active_Imobilizate = new Active_Imobilizate
        {
            Id_Companie = user.Id_Companie,
            Suma_lei = 0
        };
        _context.ActiveImobilizate.Add(active_Imobilizate);
        _context.SaveChanges();
        // Salvare Active Circulante
        Active_Circulante active_Circulante = new Active_Circulante
        {
            Id_Companie = user.Id_Companie,
            Stocuri = 0,
            Creante = 0,
            Cheltueli_inregistrate = 0,
            Numerar_Banca = 0
        };
        _context.ActiveCirculante.Add(active_Circulante);
        _context.SaveChanges();
        // Salvare Datorii
        Datorii _datorii = new Datorii
        {
            Id_Companie = user.Id_Companie,
            Datorii_Comerciale = 0,
            Datorii_Banca = 0,
            Imprumut_PTL = 0
        };
        _context.Datorii.Add(_datorii);
        _context.SaveChanges();
        // Salvare CapitaluriP
        CapitaluriiP _capitaluriiP = new CapitaluriiP
        {
            Id_Companie = user.Id_Companie,
            Capital_Social = 0,
            Profit_Nerepartizat = 0,
            Rezerve = 0
        };
        _context.CapitaluriiP.Add(_capitaluriiP);
        _context.SaveChanges();
        // Salvare Ration Financiar
        Ration_Financiar rationFinanciar = new Ration_Financiar
        {
            Id_Activ_IMT = active_Imobilizate.Id_Activ_IMT,
            Id_Active_circ = active_Circulante.Id_Active_circ,
            Id_Datorii = _datorii.Id_Datorii,
            Id_CapitaluriP = _capitaluriiP.Id_CapitaluriP,
            Solvabilitatea_Curenta = 0,
            Solvabilitatea_Generala = 0,
            Finante_Datorii = 0
        };
        _context.RationFinanciar.Add(rationFinanciar);
        _context.SaveChanges();
        
        IndicatorR indicatorR = new IndicatorR
        {
            Id_RationF = rationFinanciar.Id_RationF,
            NivelR = 0,
        };
        _context.IndicatorR.Add(indicatorR);

        _context.SaveChanges();
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