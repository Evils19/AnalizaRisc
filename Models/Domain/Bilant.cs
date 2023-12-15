using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Analiza_Risc.Models.Enum;
using Microsoft.AspNetCore.Identity;

namespace Analiza_Risc.Models;

public class Companie 
{

    [Key]
    public long Id_Companie { get; set; }
    [Required(ErrorMessage = "Numele companiei este obligatoriu")]
    public string Nume_Companie { get; set; }
    
    [Required(ErrorMessage = "CUI-ul companiei este obligatoriu")]
    public string CUI { get; set; }
    
    [Required(ErrorMessage = "Adresa companiei este obligatorie")]
    public string parola { get; set; }
    
    public Role Role { get; set; }
    public Active_Imobilizate ActiveImobilizate { get; set; }
    public Active_Circulante ActiveCirculante { get; set; }
    public Datorii Datorii { get; set; }
    public CapitaluriiP CapitaluriiP { get; set; }
    
    
}

public class Active_Imobilizate
{
    [Key]
    public long Id_Activ_IMT { get; set; }
    [ForeignKey("Id_Companie")]
    public Companie Companie { get; set; }
    public long Id_Companie { get; set; }  // Schimbați acest lucru la long, dacă este tipul corect
    public double Suma_lei { get; set; }
    public Ration_Financiar RationFinanciar { get; set; }
}


public class Active_Circulante
{
    [Key]
    public long Id_Active_circ { get; set; }

    [ForeignKey("Id_Companie")]
    public Companie Companie { get; set; }
    
    public long Id_Companie { get; set; } // Schimbați acest lucru la long, dacă este tipul corect

    public double Stocuri { get; set; }
    public double Creante { get; set; }
    public double Cheltueli_inregistrate { get; set; }
    public double Numerar_Banca { get; set; } // Numerar in banca

    public Ration_Financiar RationFinanciar { get; set; }
}


public class Datorii
{
    [Key]
    public long Id_Datorii { get; set; }
    [ForeignKey("Id_Companie")]
    public Companie Companie { get; set; }
    public long Id_Companie { get; set; }
    public double Datorii_Comerciale { get; set; }
    public double Datorii_Banca { get; set; }//Datorii fata de banca
    //Datorii pe termen long
    public double Imprumut_PTL { get; set; }//Imprumut pe termen lung
    public Ration_Financiar RationFinanciar { get; set; }
}

public class CapitaluriiP
{
    [Key]
    public long Id_CapitaluriP { get; set; }
    [ForeignKey("Id_Companie")]
    public Companie Companie { get; set; }
    public long Id_Companie { get; set; }
    public double Capital_Social { get; set; }
    public double Profit_Nerepartizat { get; set; }//Rezultat Reportat
    public double Rezerve { get; set; }
    public Ration_Financiar RationFinanciar { get; set; }
}

public class Ration_Financiar
{
    [Key]
    public long Id_RationF { get; set; }
    [ForeignKey("Id_Activ_IMT")]
    public Active_Imobilizate ActiveImobilizate { get; set; }
    public long Id_Activ_IMT { get; set; }
    [ForeignKey("Id_Active_circ")]
    public Active_Circulante ActiveCirculante { get; set; }
    public long Id_Active_circ { get; set; }
    [ForeignKey("Id_Datorii")]
    public Datorii Datorii { get; set; }
    public long Id_Datorii { get; set; }
    [ForeignKey("Id_CapitaluriP")]
    public CapitaluriiP CapitaluriiP { get; set; }
    public long Id_CapitaluriP { get; set; }
    
    
    public double Solvabilitatea_Curenta { get; set; }
    public double Solvabilitatea_Generala { get; set; }
    public double Finante_Datorii { get; set; }
    
    public IndicatorR IndicatorR { get; set; }
    
}

public class IndicatorR
{
    [Key]
    public long Id_Risc { get; set; }
    [ForeignKey("Id_RationF")]
    public Ration_Financiar RationFinanciar { get; set; }
    public long Id_RationF { get; set; }
    public double NivelR { get; set; }
}