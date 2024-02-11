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
builder.Services.AddApi(builder.Environment, builder.Configuration);
    
var app = builder.Build();

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
    app.UseSwaggerUI(swaggerUiOptions =>
    {
        swaggerUiOptions.RoutePrefix = app.Configuration.GetSection("Swagger").GetValue<string>("RoutePrefix");
    });
}

app.UseAuthentication();
app.UseAuthorization();

var apiGroup = app.MapGroup("api");

apiGroup
    .MapGroup("users")
    .MapUsersRoutes();

apiGroup
    .MapGroup("messages")
    .MapMessagesRoutes();

apiGroup
    .MapGroup("test")
    .MapTestRoutes();

apiGroup
    .MapGroup("auctions")
    .MapAuctionsRoutes();

app.Run();
