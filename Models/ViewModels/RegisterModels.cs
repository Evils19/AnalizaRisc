using System.ComponentModel.DataAnnotations;

namespace Analiza_Risc.Models.ViewModels;

public class RegisterModels
{
    [Required(ErrorMessage = "Scrieti numele companiei")]
    public string Numele_Companiei { get; set; }
    [Required(ErrorMessage = "Scrieti CUI-ul companiei")]
    public string CUI { get; set; }
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Scrieti parola care va cintine mai mult de 6 simboluri")]
    public string parola { get; set; }
    [DataType(DataType.Password)]
    public string PasswordConfirm { get; set; }
}