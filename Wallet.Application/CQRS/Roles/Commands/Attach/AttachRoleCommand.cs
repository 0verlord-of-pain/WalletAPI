﻿using MediatR;
using Wallet.Application.CQRS.Users.Queries.Views;

namespace Wallet.Application.CQRS.Roles.Commands.Attach;

public class AttachRoleCommand : IRequest<UserView>
{
    public AttachRoleCommand(Guid userId, Core.Enums.Roles role)
    {
        Role = role;
        UserId = userId;
    }

    public Core.Enums.Roles Role { get; init; }
    public Guid UserId { get; init; }
}