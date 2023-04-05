using AutoMapper;
using Wallet.Application.CQRS.Transactions.Queries.Views;
using Wallet.Application.CQRS.Users.Queries.Views;
using Wallet.Domain.Entities;

namespace Wallet.Application.Mapper;
public sealed class RegisterViews : Profile
{
    public RegisterViews()
    {
        CreateMap<User, UserView>();
        CreateMap<Transaction, TransactionView>()
            .ForMember(dest => dest.UserName,
                dest => dest
                    .MapFrom(src => src.User.UserName));
    }
}