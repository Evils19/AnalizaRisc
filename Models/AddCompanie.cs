namespace Analiza_Risc.Models;

public class AddCompanie
{
    public string Nume_Companie { get; set; }
    public int CUI { get; set; }
    public string parola { get; set; }
    
}

public class AddActiveImobilizate
{
    public double Suma_lei { get; set; }
}
public class AddActiveCirculante
{
    public double Stocuri { get; set; }
    public double Creante { get; set; }
    public double Cheltueli_inregistrate { get; set; }
    public double TotalBanca { get; set; }
    // public double NumerarBanca { get; set; }//Numerar in banca
}
public class AddDatorii
{
    public double Datorii_Comerciale { get; set; }
    public double Datorii_Banca { get; set; }//Datorii fata de banca
    //Datorii pe termen long
    public double Imprumut_PTL { get; set; }//Imprumut pe termen lung
}
public class AddCapitaluri
{
    public double Capital_Social { get; set; }
    public double Profit_Nerepartizat { get; set; }//Rezultat Reportat
    public double Rezerve { get; set; }
    
}