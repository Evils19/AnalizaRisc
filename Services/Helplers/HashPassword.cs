using System.Security.Cryptography;
using System.Text;

namespace Analiza_Risc.Services.Helplers;

public class HashPassword
{
    
    public static string Hash256(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
        
    }
}

