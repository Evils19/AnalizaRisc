using System.ComponentModel.DataAnnotations;

namespace Analiza_Risc.Models.ViewModels;

public class LoginModels
{
    [Required(ErrorMessage = "Scrieti numele companiei")]
    public string Numele_Companiei { get; set; }
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Scrieti parola")]
    [Compare("parola", ErrorMessage = "Parola nu este corecta")]
    public string parola { get; set; }
}