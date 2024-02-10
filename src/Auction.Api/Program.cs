using Auction.Api.Routes;
using Auction.Application.DependencyInjection;
using Auction.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
    
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app
    .MapGroup("users")
    .MapUsersRoutes();

app.Run();
