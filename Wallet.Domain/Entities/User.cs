﻿using Microsoft.AspNetCore.Identity;

namespace Wallet.Domain.Entities;
public class User : IdentityUser<Guid>, IBaseEntity
{
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
    public bool IsArchived { get; set; }

    public void SoftDelete()
    {
        IsArchived = true;
    }

    public void Restore()
    {
        IsArchived = false;
    }

    public static User Create(string email, string userName)
    {
        return new User
        {
            UserName = userName,
            Email = email
        };
    }
}