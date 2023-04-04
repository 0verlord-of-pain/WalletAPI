using AutoMapper;
using Wallet.Application.CQRS.Users.Queries.Views;
using Wallet.Domain.Entities;

namespace Wallet.Application.Mapper;
public sealed class RegisterViews : Profile
{
    public RegisterViews()
    {
        CreateMap<User, UserView>();
    }
}