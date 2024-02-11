using System.Reflection;
using Auction.Api.Configurations;
using Auction.Application.Common.Mediator.PipelineBehaviours;
using Auction.Application.Dtos;
using Auction.Application.Mediator.Commands.Auctions;
using Auction.Application.Common.Mediator.PipelineBehaviours;
using Auction.Application.Dtos;
using Auction.Application.Mediator.Commands.Bids;
using Auction.Application.Mediator.Commands.Lots;
using Auction.Application.Mediator.Commands.Messages;
using FastExpressionCompiler;
using Mapster;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Auction.Application.DependencyInjection;

public static class Application
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(mediatRServiceConfiguration =>
        {
            mediatRServiceConfiguration.Lifetime = ServiceLifetime.Scoped;
            
            mediatRServiceConfiguration.AddBehavior<TransactionalPipelineBehaviour<CreateMessageCommand, MessageDto>>();
            mediatRServiceConfiguration.AddBehavior<TransactionalPipelineBehaviour<CreateAuctionCommand, AuctionDto>>();
            mediatRServiceConfiguration.AddBehavior<TransactionalPipelineBehaviour<CreateAuctionCommand, AuctionDto>>();
            mediatRServiceConfiguration.AddBehavior<TransactionalPipelineBehaviour<CreateBidCommand, BidDto>>();
            mediatRServiceConfiguration.AddBehavior<TransactionalPipelineBehaviour<OpenLotCommand, Unit>>();
            mediatRServiceConfiguration.AddBehavior<TransactionalPipelineBehaviour<CloseLotCommand, Unit>>();
            
            mediatRServiceConfiguration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        
        services.AddMapster();
        Mapping.RegisterMappers();
        TypeAdapterConfig.GlobalSettings.Compiler = expression => expression.CompileFast();
        
        return services;
    }
}