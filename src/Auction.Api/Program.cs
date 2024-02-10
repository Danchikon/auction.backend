using System.Data;
using Auction.Api.Configurations;
using Auction.Api.Routes;
using Auction.Application.DependencyInjection;
using Auction.Infrastructure.DependencyInjection;
using Auction.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}
    
var app = builder.Build();

Mapping.RegisterMappers();

await using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
var context = serviceScope.ServiceProvider.GetRequiredService<AuctionDbContext>();

await context.Database.MigrateAsync();

if (context.Database.GetDbConnection() is NpgsqlConnection npgsqlConnection)
{
    if (npgsqlConnection.State is not ConnectionState.Open)
    {
        await npgsqlConnection.OpenAsync();
    }
    
    try
    {
        await npgsqlConnection.ReloadTypesAsync();
    }
    finally
    {
        await npgsqlConnection.CloseAsync();
    }

}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app
    .MapGroup("users")
    .MapUsersRoutes();

app
    .MapGroup("test")
    .MapTestRoutes();

app
    .MapGroup("auctions")
    .MapAuctionsRoutes();

app.Run();
