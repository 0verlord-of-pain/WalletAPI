using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wallet.Application.CQRS.Roles.Commands.Attach;
using Wallet.Application.CQRS.Roles.Commands.Remove;
using Wallet.Application.CQRS.Users.Queries.Views;
using Wallet.Core.Exceptions;
using Wallet.Core.Extensions;
using Wallet.Domain.Entities;
using Wallet.Storage.Persistence;

namespace Wallet.Application.CQRS.Roles.Commands;

public class RoleCommandHandler :
    IRequestHandler<AttachRoleCommand, UserView>,
    IRequestHandler<RemoveRoleCommand, UserView>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public RoleCommandHandler(UserManager<User> userManager, IMapper mapper, DataContext context)
    {
        _userManager = userManager;
        _mapper = mapper;
        _context = context;
    }

    public async Task<UserView> Handle(AttachRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        var addToRoleResult = await _userManager.AddToRoleAsync(user, request.Role.ToString());
        if (addToRoleResult.TryGetErrors(out var addToRoleErrors)) throw new IdentityUserException(addToRoleErrors);

        var view = _mapper.Map<UserView>(user);

        var rolesIds = _context.UserRoles
            .Where(i => i.UserId.Equals(view.Id))
            .Select(i => i.RoleId);

        view.Roles = await _context.Roles
            .Where(i => rolesIds.Contains(i.Id))
            .Select(i => i.Name)
            .ToArrayAsync(cancellationToken);
        return view;
    }

    public async Task<UserView> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        var addToRoleResult = await _userManager.RemoveFromRoleAsync(user, request.Role.ToString());
        if (addToRoleResult.TryGetErrors(out var addToRoleErrors)) throw new IdentityUserException(addToRoleErrors);

        var view = _mapper.Map<UserView>(user);

        var rolesIds = _context.UserRoles
            .Where(i => i.UserId.Equals(view.Id))
            .Select(i => i.RoleId);

        view.Roles = await _context.Roles
            .Where(i => rolesIds.Contains(i.Id))
            .Select(i => i.Name)
            .ToArrayAsync(cancellationToken);
        return view;
    }
}