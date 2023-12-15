using System.ComponentModel.DataAnnotations;

namespace Analiza_Risc.Models.ViewModels;

public class LoginModels
{
    [Required(ErrorMessage = "Scrieti numele companiei")]
    public string Numele_Companiei { get; set; }
    [Required(ErrorMessage = "Scrieti parola")]
    public string parola { get; set; }
}