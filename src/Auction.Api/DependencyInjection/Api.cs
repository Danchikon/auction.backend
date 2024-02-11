using Auction.Infrastructure.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Auction.Api.DependencyInjection;

public static class Api
{
    public static IServiceCollection AddApi(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
    {
        if (environment.IsDevelopment())
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(swaggerGenOptions =>
            {
                var swaggerOptions = configuration
                    .GetSection(SwaggerOptions.Section)
                    .Get<SwaggerOptions>()!;
                
                foreach (var serverUrl in swaggerOptions.ServersUrls)
                {
                    swaggerGenOptions.AddServer(new OpenApiServer
                    {
                        Url = serverUrl
                    });
                }
                
                swaggerGenOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    In = ParameterLocation.Header, 
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey 
                });
                swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    { 
                        new OpenApiSecurityScheme 
                        { 
                            Reference = new OpenApiReference 
                            { 
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer" 
                            } 
                        },
                        new string[] { } 
                    } 
                });
            });
        }

        using var serviceProvider = services.BuildServiceProvider();

        var jsonWebKey = serviceProvider.GetRequiredService<JsonWebKey>();

        services
            .AddAuthentication()
            .AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters.IssuerSigningKey = jsonWebKey;
                jwtBearerOptions.TokenValidationParameters.ValidIssuer = "auction";
                jwtBearerOptions.TokenValidationParameters.ValidAudience = "auction";
                jwtBearerOptions.RequireHttpsMetadata = false;
            });
        
        services.AddAuthorization();

        return services;
    }
}