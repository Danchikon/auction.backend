using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Auction.Api.DependencyInjection;

public static class Api
{
    public static IServiceCollection AddApi(this IServiceCollection services, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(swaggerGenOptions =>
            {
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

        services
            .AddAuthentication()
            .AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters.IssuerSigningKey = new JsonWebKey("{\n    \"crv\": \"P-256\",\n    \"ext\": true,\n    \"key_ops\": [\n        \"verify\"\n    ],\n    \"kid\": \"5LICTzyWHEP9Op58queF3EsbvxuL6vuvmwuamOzPD_A\",\n    \"kty\": \"EC\",\n    \"x\": \"MaiR_PVaV-EYlhQcBdA6dVqnlRGMXUihqZ-rEjQAq18\",\n    \"y\": \"o3M5JCV4xkUzzyfmhjqHTpoY09SEcZyzoa4f0MB_380\"\n}");
                jwtBearerOptions.TokenValidationParameters.ValidIssuer = "auction";
                jwtBearerOptions.TokenValidationParameters.ValidAudience = "auction";
                jwtBearerOptions.RequireHttpsMetadata = false;
            });
        
        services.AddAuthorization();

        return services;
    }
}