using System.ComponentModel.DataAnnotations;

namespace Analiza_Risc.Models.Enum;

public enum Role
{
    [Display(Name = "Administrator")]
    admin = 2,
    [Display(Name = "Utilizator")]
    user = 1
}