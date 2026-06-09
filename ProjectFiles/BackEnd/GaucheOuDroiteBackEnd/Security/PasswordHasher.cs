using Microsoft.AspNetCore.Identity;

namespace GaucheOuDroiteBackEnd.Security;

public class PasswordHasher
{
    readonly PasswordHasher<object> _hasher = new();

    public string HashPassword(string p_password)
    {
        return _hasher.HashPassword(
            null!,
            p_password
        );
    }

    public bool VerifyHashedPassword(string p_hashedPassword, string p_password)
    {
        PasswordVerificationResult result = _hasher.VerifyHashedPassword(
            null!,
            p_hashedPassword,
            p_password
        );

        return result == PasswordVerificationResult.Success;
    }
}