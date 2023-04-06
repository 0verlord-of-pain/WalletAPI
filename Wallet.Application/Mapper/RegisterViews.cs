using AutoMapper;
using Wallet.Application.CQRS.CardsBalance.Queries.Views;
using Wallet.Application.CQRS.Transactions.Queries.Views;
using Wallet.Application.CQRS.Users.Queries.Views;
using Wallet.Core.Enums;
using Wallet.Domain.Entities;

namespace Wallet.Application.Mapper;

public sealed class RegisterViews : Profile
{
    public RegisterViews()
    {
        CreateMap<User, UserView>()
            .ForMember(dest => dest.CardBalanceId,
                dest => dest
                    .MapFrom(src => src.CardBalance.Id));

        CreateMap<Transaction, TransactionView>()
            .ForMember(dest => dest.Amount,
                dest => dest
                    .MapFrom(src => src.Type == TransactionType.Payment ? $"+{src.Amount}" : src.Amount.ToString()))
            .ForMember(dest => dest.UserName,
                dest => dest
                    .MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.CreatedOnUtc,
                dest => dest
                    .MapFrom(src =>
                        src.CreatedOnUtc >= new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month,DateTime.UtcNow.Day).AddDays(-7)
                            ? src.CreatedOnUtc.DayOfWeek.ToString()
                            : src.CreatedOnUtc.ToString("MM/dd/yyyy, hh:mm")));

        CreateMap<CardBalance, CardBalanceView>();
    }
}