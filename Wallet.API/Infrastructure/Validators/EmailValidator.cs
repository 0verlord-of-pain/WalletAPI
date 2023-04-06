using System.ComponentModel.DataAnnotations;

namespace Wallet.API.Infrastructure.Validators;

public struct EmailValidator
{
    public static bool IsValid(string email)
    {
        return new EmailAddressAttribute().IsValid(email);
    }
}