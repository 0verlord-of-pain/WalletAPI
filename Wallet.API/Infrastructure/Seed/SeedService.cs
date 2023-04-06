using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using Wallet.Core.Enums;
using Wallet.Domain.Entities;
using Wallet.Storage.Persistence;

namespace Wallet.API.Infrastructure.Seed;

internal sealed class SeedService : ISeedService
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly DataContext _context;

    public SeedService(
        RoleManager<IdentityRole<Guid>> roleManager,
        UserManager<User> userManager,
        DataContext context)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _context = context;
    }

    public async Task SeedRolesAsync()
    {
        await _roleManager.CreateAsync(new IdentityRole<Guid>(Roles.Admin.ToString()));
        await _roleManager.CreateAsync(new IdentityRole<Guid>(Roles.Manager.ToString()));
        await _roleManager.CreateAsync(new IdentityRole<Guid>(Roles.User.ToString()));
    }

    public async Task SeedAdminAndManagerAsync()
    {
        var defaultAdmin = new User
        {
            UserName = "admin",
            Email = "admin@gmail.com",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        if (_userManager.Users.All(u => u.Email != defaultAdmin.Email))
        {
            var user = await _userManager.FindByEmailAsync(defaultAdmin.Email);
            if (user is null)
            {
                await _userManager.CreateAsync(defaultAdmin, "admin1");
                await _userManager.AddToRoleAsync(defaultAdmin, Roles.Admin.ToString());
                await _userManager.AddToRoleAsync(defaultAdmin, Roles.Manager.ToString());
                await _userManager.AddToRoleAsync(defaultAdmin, Roles.User.ToString());
            }
        }

        defaultAdmin = await _context.Users
            .FirstOrDefaultAsync(i=>i.Email ==defaultAdmin.Email, CancellationToken.None);

        if (defaultAdmin != null)
        {
            defaultAdmin.CardBalance = CardBalance.Create(defaultAdmin);
            await _context.SaveChangesAsync();
            defaultAdmin.CardBalanceId = defaultAdmin.CardBalance.Id;
        }

        var defaultManager = new User
        {
            UserName = "manager",
            Email = "manager@gmail.com",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        if (_userManager.Users.All(u => u.Email != defaultManager.Email))
        {
            var user = await _userManager.FindByEmailAsync(defaultManager.Email);
            if (user is null)
            {
                await _userManager.CreateAsync(defaultManager, "manager1");
                await _userManager.AddToRoleAsync(defaultManager, Roles.Manager.ToString());
                await _userManager.AddToRoleAsync(defaultManager, Roles.User.ToString());
            }
        }
        defaultManager = await _context.Users
            .FirstOrDefaultAsync(i => i.Email == defaultManager.Email, CancellationToken.None);

        if (defaultManager != null)
        {
            defaultManager.CardBalance = CardBalance.Create(defaultManager);
            await _context.SaveChangesAsync();
            defaultManager.CardBalanceId = defaultManager.CardBalance.Id;
        }

        await _context.SaveChangesAsync();
    }

    public async Task SeedDefaultUserAsync()
    {
        var defaultUser = new User
        {
            UserName = "Test",
            Email = "test@gmail.com",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        if (_userManager.Users.All(u => u.Email != defaultUser.Email))
        {
            var user = await _userManager.FindByEmailAsync(defaultUser.Email);
            if (user is null)
            {
                await _userManager.CreateAsync(defaultUser, "test12");
                await _userManager.AddToRoleAsync(defaultUser, Roles.User.ToString());
            }
        }
        defaultUser = await _context.Users
            .FirstOrDefaultAsync(i => i.Email == defaultUser.Email, CancellationToken.None);

        if (defaultUser != null)
        {
            var cardBalance = CardBalance.Create(defaultUser);
            cardBalance.Balance = 154;
            _context.CardsBalance.Add(cardBalance);
            await _context.SaveChangesAsync();

            defaultUser.CardBalanceId = defaultUser.CardBalance.Id;
            await _context.SaveChangesAsync();

            var transaction = Transaction.Create(TransactionType.Payment, 10, "Test", "PaymentOk", TransactionStatus.Ok, defaultUser, "https://otkritkis.com/wp-content/uploads/2021/12/img1991383_vyisokiy_kreditnyiy_reyting_Vsemirnogo_banka-640x475-1.jpg");
            _context.Transactions.Add(transaction);

            cardBalance.ChangeBalance(10);

            await _context.SaveChangesAsync();

            transaction = Transaction.Create(TransactionType.Credit, 57, "Test", "CreditOk", TransactionStatus.Ok, defaultUser, "https://otkritkis.com/wp-content/uploads/2021/12/img1991383_vyisokiy_kreditnyiy_reyting_Vsemirnogo_banka-640x475-1.jpg");
            _context.Transactions.Add(transaction);

            cardBalance.ChangeBalance(-57);

            await _context.SaveChangesAsync();

            transaction = Transaction.Create(TransactionType.Payment, 69, "Test", "PaymentPending", TransactionStatus.Pending, defaultUser, "https://otkritkis.com/wp-content/uploads/2021/12/img1991383_vyisokiy_kreditnyiy_reyting_Vsemirnogo_banka-640x475-1.jpg");
            _context.Transactions.Add(transaction);

            cardBalance.ChangeBalance(69);

            await _context.SaveChangesAsync();
        }

    }
}