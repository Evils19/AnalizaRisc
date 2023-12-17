using System.Security.Claims;
using Analiza_Risc.Models;
using Analiza_Risc.Data;
using Analiza_Risc.Response;
using Microsoft.AspNetCore.Authorization;
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


    private double CalculeazaNivelRisc(double solvabilitateCurenta, double solvabilitateGenerala, double finanteDatorii)
    {
        // Ponderi pentru fiecare indicator
        double pondereSolvabilitateCurenta = 0.4;
        double pondereSolvabilitateGenerala = 0.4;
        double pondereFinanteDatorii = 0.2;

        // Calculul scorului total
        double scorTotal = (solvabilitateCurenta * pondereSolvabilitateCurenta) +
                           (solvabilitateGenerala * pondereSolvabilitateGenerala) +
                           (finanteDatorii * pondereFinanteDatorii);

        // Atribuirea unui nivel general de risc în funcție de scorul total
        double nivelRisc;

        if (scorTotal < 30)
        {
            nivelRisc = 10; // Nivel scăzut de risc
        }
        else if (scorTotal < 60)
        {
            nivelRisc = 50; // Nivel moderat de risc
        }
        else
        {
            nivelRisc = 90; // Nivel ridicat de risc
        }

        return nivelRisc;
    }

    public void InregisrCompanie(ClaimsIdentity response, AddActiveImobilizate activeImobilizate,
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
        _context.ActiveImobilizate.Add(active_Imobilizate);
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
        _context.ActiveCirculante.Add(active_Circulante);
        _context.SaveChanges();
        // Salvare Datorii
        Datorii _datorii = new Datorii
        {
            Id_Companie = companie.Id_Companie,
            Datorii_Comerciale = addDatorii.Datorii_Comerciale,
            Datorii_Banca = addDatorii.Datorii_Banca,
            Imprumut_PTL = addDatorii.Imprumut_PTL
        };
        _context.Datorii.Add(_datorii);
        _context.SaveChanges();
        // Salvare CapitaluriP
        CapitaluriiP _capitaluriiP = new CapitaluriiP
        {
            Id_Companie = companie.Id_Companie,
            Capital_Social = addCapitaluri.Capital_Social,
            Profit_Nerepartizat = addCapitaluri.Profit_Nerepartizat,
            Rezerve = addCapitaluri.Rezerve
        };
        _context.CapitaluriiP.Add(_capitaluriiP);
        _context.SaveChanges();
        // Salvare Ration Financiar
        var Active_Circulante1 = active_Circulante.Stocuri + active_Circulante.Creante +
                                 active_Circulante.Cheltueli_inregistrate + active_Circulante.Numerar_Banca;
        var Total_Datorii = _datorii.Datorii_Comerciale + _datorii.Datorii_Banca + _datorii.Imprumut_PTL;
        var Total_Active = active_Imobilizate.Suma_lei + Active_Circulante1;
        Ration_Financiar rationFinanciar = new Ration_Financiar
        {
            Id_Activ_IMT = active_Imobilizate.Id_Activ_IMT,
            Id_Active_circ = active_Circulante.Id_Active_circ,
            Id_Datorii = _datorii.Id_Datorii,
            Id_CapitaluriP = _capitaluriiP.Id_CapitaluriP,
            Solvabilitatea_Curenta = Active_Circulante1 / (_datorii.Datorii_Comerciale + _datorii.Datorii_Banca),
            Solvabilitatea_Generala = Total_Active / Total_Datorii,
            Finante_Datorii = Total_Datorii / (_capitaluriiP.Capital_Social + _capitaluriiP.Profit_Nerepartizat +
                                               _capitaluriiP.Rezerve)
        };
        _context.RationFinanciar.Add(rationFinanciar);
        _context.SaveChanges();

        // Salvare Indicator R
        var nivelRisc = CalculeazaNivelRisc(rationFinanciar.Solvabilitatea_Curenta,
            rationFinanciar.Solvabilitatea_Generala, rationFinanciar.Finante_Datorii);

        IndicatorR indicatorR = new IndicatorR
        {
            Id_RationF = rationFinanciar.Id_RationF,
            NivelR = nivelRisc
        };
        _context.IndicatorR.Add(indicatorR);

        _context.SaveChanges();

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
   
    if (activeImobilizate == null || activeCirculante == null || Datorii == null || Capitaluri == null)
    {
        activeImobilizate = new Active_Imobilizate
        {
            Suma_lei = 0
        };
        activeCirculante = new Active_Circulante
        {
            Stocuri = 0,
            Creante = 0,
            Cheltueli_inregistrate = 0,
            Numerar_Banca = 0
        };
        Datorii = new Datorii
        {
            Datorii_Comerciale = 0,
            Datorii_Banca = 0,
            Imprumut_PTL = 0
        };
        Capitaluri = new CapitaluriiP
        {
            Capital_Social = 0,
            Profit_Nerepartizat = 0,
            Rezerve = 0
        };
         IndicatorR = new IndicatorR
        {
            NivelR = 0
        };
    }

    RationFinanciar = new Ration_Financiar();
    



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

}