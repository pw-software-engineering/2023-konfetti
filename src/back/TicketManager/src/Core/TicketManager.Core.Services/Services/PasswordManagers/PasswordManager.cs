using System.Security.Cryptography;

namespace TicketManager.Core.Services.Services.PasswordManagers;

public class PasswordManager
{
    public const int SaltLength = 16;
    public const int HashLength = 20;
    
    public string GetHash(string password)
    {
        var salt = new byte[SaltLength];
        var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(salt);
        
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
        var hash = pbkdf2.GetBytes(HashLength);
        
        var hashBytes = new byte[SaltLength + HashLength];
        Array.Copy(salt, 0, hashBytes, 0, SaltLength);
        Array.Copy(hash, 0, hashBytes, SaltLength, HashLength);
        
        return Convert.ToBase64String(hashBytes);
    }

    public bool DoPasswordsMatch(string passwordHash, string password)
    {
        var hashBytes = Convert.FromBase64String(passwordHash);
        
        var salt = new byte[SaltLength];
        Array.Copy(hashBytes, 0, salt, 0, SaltLength);
        
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
        var hash = pbkdf2.GetBytes(HashLength);
        
        for (int i=0; i < HashLength; i++)
            if (hashBytes[i + SaltLength] != hash[i])
                return false;

        return true;
    }
}
