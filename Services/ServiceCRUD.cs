using System.Security.Claims;
using Analiza_Risc.Models;
using Analiza_Risc.Data;
using Analiza_Risc.Models.Enum;
using Analiza_Risc.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Analiza_Risc.Services;

[Authorize]
public class ServiceCRUD : IServiceCRUD
{

    private readonly AppDbContext _context;

    public ServiceCRUD(AppDbContext context)
    {
        _context = context;

    }

    public double CalculeazaNivelRisc(double solvabilitateCurenta, double solvabilitateGeneral, double finanteDatorii)
    {
        // Multiplicăm cu 10 pentru a obține un scor pe o scară de la 0 la 10
        double scorSolvabilitateCurenta = solvabilitateCurenta * 10;
        double scorSolvabilitateGeneral = solvabilitateGeneral * 10;
        double scorFinanteDatorii = finanteDatorii * 10;

        // Calculăm scorul mediu
        double scorMediu = (scorSolvabilitateCurenta + scorSolvabilitateGeneral + scorFinanteDatorii) / 3;

        // Limităm scorul final la două cifre după virgulă
        double scorFinal = Math.Round(scorMediu, 2);

        // Întoarceți scorul final
        return scorFinal;
    }
    private string GenereazaFeedback(double procentajRisc)
    {
        string feedback = "";

        if (procentajRisc <= 30.0)
        {
            feedback = "Risc scăzut. Compania are o solvabilitate bună și finanțe sănătoase. Recomandăm să mențineți practicile financiare bune și să continuați monitorizarea regulată.";
        }
        else if (procentajRisc <= 60.0)
        {
            feedback = "Risc moderat. Se recomandă monitorizarea atentă a situației financiare. Evaluarea regulată a datoriilor și creșterea eficienței operaționale pot contribui la reducerea riscului.";
        }
        else if (procentajRisc <= 80.0)
        {
            feedback = "Risc ridicat. Compania întâmpină dificultăți financiare semnificative. Recomandăm o analiză amănunțită a sumelor datorate și a cheltuielilor. Implementarea unor strategii de reducere a datoriilor și optimizarea costurilor ar putea fi esențiale pentru îmbunătățirea situației financiare.";
        }
        else
        {
            feedback = "Risc foarte ridicat. Compania se află într-o situație financiară critică. Este vitală o evaluare profundă și imediată a tuturor aspectelor financiare. Recomandăm consultarea cu un expert financiar și luarea de măsuri urgente pentru a evita problemele majore.";
        }

        return feedback;
    }



   public async Task<IBaseResponse<Companie>> Update(ClaimsIdentity response, AddActiveImobilizate activeImobilizate,
        AddActiveCirculante activeCirculante, AddDatorii addDatorii, AddCapitaluri addCapitaluri)
    {


        var user = response.Name;
        var companie = _context.Companies.FirstOrDefault(x => x.Nume_Companie == user);
        
        // Salvare Active Imobilizate
        Active_Imobilizate active_Imobilizate = new Active_Imobilizate
        {
            Id_Companie = companie.Id_Companie,
            Suma_lei = activeImobilizate.Suma_lei
        };
        var bd_ActiveImobilizate = _context.ActiveImobilizate.FirstOrDefault(x=>x.Id_Companie == companie.Id_Companie);
        bd_ActiveImobilizate.Suma_lei = activeImobilizate.Suma_lei;
       
        _context.ActiveImobilizate.Update(bd_ActiveImobilizate);
        _context.SaveChanges();
        
        // Salvare Active Circulante
        Active_Circulante active_Circulante = new Active_Circulante
        {
            Id_Companie = companie.Id_Companie,
            Stocuri = activeCirculante.Stocuri,
            Creante = activeCirculante.Creante,
            Cheltueli_inregistrate = activeCirculante.Cheltueli_inregistrate,
            Numerar_Banca = activeCirculante.TotalBanca
        };
        
      var bd_ActiveCirculante = _context.ActiveCirculante.FirstOrDefault(x=>x.Id_Companie == companie.Id_Companie);
      bd_ActiveCirculante.Stocuri = activeCirculante.Stocuri;
      bd_ActiveCirculante.Creante = activeCirculante.Creante;
      bd_ActiveCirculante.Cheltueli_inregistrate = activeCirculante.Cheltueli_inregistrate;
      bd_ActiveCirculante.Numerar_Banca = activeCirculante.TotalBanca;
        _context.ActiveCirculante.Update(bd_ActiveCirculante);
        _context.SaveChanges();
        
        // Salvare Datorii
        Datorii _datorii = new Datorii
        {
            Id_Companie = companie.Id_Companie,
            Datorii_Comerciale = addDatorii.Datorii_Comerciale,
            Datorii_Banca = addDatorii.Datorii_Banca,
            Imprumut_PTL = addDatorii.Imprumut_PTL
        };
        var bd_Datorii = _context.Datorii.FirstOrDefault(x=>x.Id_Companie == companie.Id_Companie);
        bd_Datorii.Datorii_Comerciale = addDatorii.Datorii_Comerciale;
        bd_Datorii.Datorii_Banca = addDatorii.Datorii_Banca;
        bd_Datorii.Imprumut_PTL = addDatorii.Imprumut_PTL;
        _context.Datorii.Update(bd_Datorii);
        _context.SaveChanges();
        
        // Salvare CapitaluriP
        CapitaluriiP _capitaluriiP = new CapitaluriiP
        {
            Id_Companie = companie.Id_Companie,
            Capital_Social = addCapitaluri.Capital_Social,
            Profit_Nerepartizat = addCapitaluri.Profit_Nerepartizat,
            Rezerve = addCapitaluri.Rezerve
        };
        
        var bd_CapitaluriiP = _context.CapitaluriiP.FirstOrDefault(x=>x.Id_Companie == companie.Id_Companie);
        bd_CapitaluriiP.Capital_Social = addCapitaluri.Capital_Social;
        bd_CapitaluriiP.Profit_Nerepartizat = addCapitaluri.Profit_Nerepartizat;
        bd_CapitaluriiP.Rezerve = addCapitaluri.Rezerve;
       
        _context.CapitaluriiP.Update(bd_CapitaluriiP);
        _context.SaveChanges();
     
        var Active_Circulante1 = bd_ActiveCirculante.Stocuri + bd_ActiveCirculante.Creante +
                                 bd_ActiveCirculante.Cheltueli_inregistrate + bd_ActiveCirculante.Numerar_Banca;
       
        var Total_Datorii = bd_Datorii.Datorii_Comerciale + bd_Datorii.Datorii_Banca + bd_Datorii.Imprumut_PTL;
   
        var Total_Active =bd_ActiveImobilizate.Suma_lei + Active_Circulante1;
     
        
        var bd_RationFinanciar = _context.RationFinanciar.FirstOrDefault(x=>x.Id_Datorii == bd_Datorii.Id_Datorii);
        bd_RationFinanciar.Solvabilitatea_Curenta = Active_Circulante1 / (_datorii.Datorii_Comerciale + _datorii.Datorii_Banca);
        bd_RationFinanciar.Solvabilitatea_Generala = Total_Active / Total_Datorii;
        bd_RationFinanciar.Finante_Datorii = Total_Datorii /
                                             (_capitaluriiP.Capital_Social + _capitaluriiP.Profit_Nerepartizat +
                                              _capitaluriiP.Rezerve);
      _context.RationFinanciar.Update(bd_RationFinanciar);
        _context.SaveChanges();
        
        // Salvare Indicator R
        var nivelRisc = CalculeazaNivelRisc(bd_RationFinanciar.Solvabilitatea_Curenta,
            bd_RationFinanciar.Solvabilitatea_Generala, bd_RationFinanciar.Finante_Datorii);
        
        var feedback = GenereazaFeedback(nivelRisc);
   
        IndicatorR indicatorR = new IndicatorR
        {
            Id_RationF = bd_RationFinanciar.Id_RationF,
            NivelR = nivelRisc
        };
        
        var bd_IndicatorR = _context.IndicatorR.FirstOrDefault(x=>x.Id_RationF == bd_RationFinanciar.Id_RationF);
        bd_IndicatorR.NivelR = nivelRisc;
        _context.IndicatorR.Update(bd_IndicatorR);
        _context.SaveChanges();
        
        return new BaseResponse<Companie>()
        {
            Data = companie,
            Description = feedback,
            StatusCode = StatusCodeUser.ok
        };
        
        
    }
    


public async Task<InfoData> GetInfo(ClaimsIdentity response)
{
    var user = response.Name;
    var companie = await _context.Companies.FirstOrDefaultAsync(x => x.Nume_Companie == user);
    var activeImobilizate = await _context.ActiveImobilizate.FirstOrDefaultAsync(x => x.Id_Companie == companie.Id_Companie);
    var activeCirculante = await _context.ActiveCirculante.FirstOrDefaultAsync(x => x.Id_Companie == companie.Id_Companie);
    var Datorii = await _context.Datorii.FirstOrDefaultAsync(x => x.Id_Companie == companie.Id_Companie);
    var Capitaluri = await _context.CapitaluriiP.FirstOrDefaultAsync(x => x.Id_Companie == companie.Id_Companie);
    var RationFinanciar = await _context.RationFinanciar.FirstOrDefaultAsync(x => x.Id_Activ_IMT == activeImobilizate.Id_Activ_IMT);
    var IndicatorR = await _context.IndicatorR.FirstOrDefaultAsync(x => x.Id_RationF == RationFinanciar.Id_RationF);
    


    return new InfoData
    {
        ActiveImobilizate = activeImobilizate,
        ActiveCirculante = activeCirculante,
        Datorii = Datorii,
        Capitaluri = Capitaluri,
        RationFinanciar = RationFinanciar,
        IndicatorR = IndicatorR
    };
}



public async Task<InfoData> GetCompani(ClaimsIdentity response)
{
    var user = response.Name;
    var companie = await _context.Companies.FirstOrDefaultAsync(x => x.Nume_Companie == user);
    var activeImobilizate = await _context.ActiveImobilizate.FirstOrDefaultAsync(x => x.Id_Companie == companie.Id_Companie);
    var activeCirculante = await _context.ActiveCirculante.FirstOrDefaultAsync(x => x.Id_Companie == companie.Id_Companie);
    var Datorii = await _context.Datorii.FirstOrDefaultAsync(x => x.Id_Companie == companie.Id_Companie);
    var Capitaluri = await _context.CapitaluriiP.FirstOrDefaultAsync(x => x.Id_Companie == companie.Id_Companie);


   return new InfoData()
   {
       Companie = companie,
       ActiveImobilizate = activeImobilizate,
       ActiveCirculante = activeCirculante,
       Datorii = Datorii,
       Capitaluri = Capitaluri,
    };
   }




public void Delete(ClaimsIdentity response)
{
    var user = response.Name;

    // Find the company based on the user name
    var companie = _context.Companies
        .Include(c => c.ActiveImobilizate)
            .ThenInclude(ai => ai.RationFinanciar)
                .ThenInclude(rf => rf.IndicatorR)
        .Include(c => c.ActiveCirculante)
        .Include(c => c.Datorii)
        .Include(c => c.CapitaluriiP)
        .FirstOrDefault(x => x.Nume_Companie == user);

    if (companie == null)
    {
       
        Console.WriteLine("Compania nu a fost gasita");
    }

    try
    {
        // Remove related entities
        if (companie.ActiveImobilizate != null)
        {
            _context.IndicatorR.Remove(companie.ActiveImobilizate.RationFinanciar.IndicatorR);
            _context.RationFinanciar.Remove(companie.ActiveImobilizate.RationFinanciar);
        }
        
        _context.ActiveImobilizate.Remove(companie.ActiveImobilizate);
        _context.ActiveCirculante.Remove(companie.ActiveCirculante);
        _context.Datorii.Remove(companie.Datorii);
        _context.CapitaluriiP.Remove(companie.CapitaluriiP);

        
        _context.Companies.Remove(companie);

  
        _context.SaveChanges();

        
        Console.WriteLine("Compania a fost stearsa cu succes");
    }
    catch (Exception ex)
    {
        
        Console.WriteLine($"Error deleting company: {ex.Message}");
    }
}



}





