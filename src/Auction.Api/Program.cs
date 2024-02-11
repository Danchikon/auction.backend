using System.Data;
using Auction.Api.Configurations;
using Auction.Api.DependencyInjection;
using Auction.Api.Routes;
using Auction.Application.DependencyInjection;
using Auction.Infrastructure.DependencyInjection;
using Auction.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Host.ConfigureSerilog();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddApi(builder.Environment);
    
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

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

#region Routing

app
    .MapGroup("users")
    .MapUsersRoutes();

app
    .MapGroup("messages")
    .MapMessagesRoutes();

app
    .MapGroup("test")
    .MapTestRoutes();

app
    .MapGroup("auctions")
    .MapAuctionsRoutes();

app
    .MapGroup("lots")
    .MapLotsRoutes();

#endregion

app.Run();
