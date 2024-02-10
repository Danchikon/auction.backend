using System.Reflection;
using Auction.Application.Abstractions;
using Auction.Domain.Common;
using Auction.Domain.Entities;
using Auction.Domain.Enums;
using Auction.Infrastructure.Implementations;
using Auction.Infrastructure.Options;
using Auction.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Minio;
using Npgsql;

namespace Auction.Infrastructure.DependencyInjection;

public static class Infrastructure
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddMinioFilesStorage();

        services.AddScoped<IEventsPublisher, CentrifugoOutboxEventsPublisher<AuctionDbContext>>();
        services.AddScoped<IRepository<UserEntity>, EfRepository<UserEntity, AuctionDbContext>>();        
        services.AddScoped<IRepository<MessageEntity>, EfRepository<MessageEntity, AuctionDbContext>>();

        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddJsonWebTokenService();
                                     
        services.AddPostgresDataSource(dataSourceBuilderConfigurator =>
        {
            dataSourceBuilderConfigurator.MapEnum<AuctionState>();
            dataSourceBuilderConfigurator.MapEnum<LotState>();                              
        });
        
        services.AddEfPostgres<AuctionDbContext>();
        
        services.AddEfUnitOfWork<AuctionDbContext>();
        
        return services;
    }
    
    public static IServiceCollection AddJsonWebTokenService(this IServiceCollection services)
    {
        services.AddSingleton<SigningCredentials>(provider =>
        {
            var jsonWebKey = new JsonWebKey("{\n    \"crv\": \"P-256\",\n    \"d\": \"4jUUtEizHhny1QOMp030Oed4BwIyMLRaQAMloJ4_Fa8\",\n    \"ext\": true,\n    \"key_ops\": [\n        \"sign\"\n    ],\n    \"kid\": \"5LICTzyWHEP9Op58queF3EsbvxuL6vuvmwuamOzPD_A\",\n    \"kty\": \"EC\",\n    \"x\": \"MaiR_PVaV-EYlhQcBdA6dVqnlRGMXUihqZ-rEjQAq18\",\n    \"y\": \"o3M5JCV4xkUzzyfmhjqHTpoY09SEcZyzoa4f0MB_380\"\n}");
            
            var signingCredentials = new SigningCredentials(jsonWebKey, SecurityAlgorithms.EcdsaSha256);

            return signingCredentials;
        });
        
        services.AddSingleton<JsonWebTokenHandler>(provider =>
        {
            var tokenHandler = new JsonWebTokenHandler
            {
                TokenLifetimeInMinutes = Convert.ToInt32(TimeSpan.FromDays(30).TotalMinutes)
            };

            return tokenHandler;
        });
        
        services.AddSingleton<JsonWebTokenService>();

        return services;
    }
    
    public static IServiceCollection AddMinioFilesStorage(this IServiceCollection services)
    {
        services
            .AddOptions<MinioOptions>()
            .BindConfiguration(MinioOptions.Section);
        
        services.AddSingleton<IMinioClient>(provider =>
        {
            var minioOptions = provider.GetRequiredService<IOptions<MinioOptions>>().Value;
            
            var client = new MinioClient()
                .WithEndpoint(minioOptions.Endpoint)
                .WithCredentials(minioOptions.AccessKey,  minioOptions.SecretKey)
                .Build();

            return client;
        });
        services.AddSingleton<IFilesStorage, MinioFilesStorage>();

        return services;
    }
    
    public static IServiceCollection AddEfPostgres<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
    {
        const string postgresMigrationsTableName = "ef_migrations";
        
        services.AddEf<TDbContext>((provider, dbContextOptionsBuilder) =>
        {
            var dataSourceBuilder = provider.GetRequiredService<NpgsqlDataSource>();
            
            dbContextOptionsBuilder.UseNpgsql(dataSourceBuilder, npgsqlDbContextOptionsBuilder =>
            {
                npgsqlDbContextOptionsBuilder.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                npgsqlDbContextOptionsBuilder.MigrationsHistoryTable(postgresMigrationsTableName);
            });
        });

        return services;
    }
    
    public static IServiceCollection AddEf<TDbContext>(
        this IServiceCollection services, 
        Action<IServiceProvider, DbContextOptionsBuilder>? dbContextOptionsBuilderConfigurator = null
    ) where TDbContext : DbContext
    {
        services.AddPooledDbContextFactory<TDbContext>((provider, dbContextOptionsBuilder) =>
        {
            var environment = provider.GetRequiredService<IHostEnvironment>();
           
            if (environment.IsDevelopment())
            {
                dbContextOptionsBuilder.EnableSensitiveDataLogging();
                dbContextOptionsBuilder.EnableDetailedErrors();
            }
            
            dbContextOptionsBuilder.UseSnakeCaseNamingConvention();
            dbContextOptionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            
            dbContextOptionsBuilderConfigurator?.Invoke(provider, dbContextOptionsBuilder);
        });

        services.AddScoped<TDbContext>(provider =>
        {
            var dbContextFactory = provider.GetRequiredService<IDbContextFactory<TDbContext>>();
            
            var dbContext = dbContextFactory.CreateDbContext();
            
            return dbContext;
        });

        return services;
    }
    
    public static IServiceCollection AddEfUnitOfWork<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
    {
        services.AddScoped<IUnitOfWork, EfUnitOfWork<TDbContext>>();

        return services;
    }
    
    public static IServiceCollection AddPostgresDataSource(
        this IServiceCollection services, 
        Action<NpgsqlDataSourceBuilder>? dataSourceBuilderConfigurator = null
    ) 
    {
        const string postgresConnectionStringName = "Postgres";
        
        services.AddSingleton(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var environment = provider.GetRequiredService<IHostEnvironment>();
            var connectionString = configuration.GetConnectionString(postgresConnectionStringName);
            
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

            if (environment.IsDevelopment())
            {
                dataSourceBuilder.EnableParameterLogging();
            }
            
            dataSourceBuilderConfigurator?.Invoke(dataSourceBuilder);

            var dataSource = dataSourceBuilder.Build();
            
            return dataSource;
        });
        
        return services;
    }
}