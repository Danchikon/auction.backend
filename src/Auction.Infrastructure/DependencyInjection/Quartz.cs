using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.AspNetCore;

namespace Auction.Infrastructure.DependencyInjection;

public static class Quartz
{
    public static IServiceCollection AddQuartzScheduling(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(serviceCollectionQuartzConfigurator =>
        {
            serviceCollectionQuartzConfigurator.UsePersistentStore(persistentStoreOptions =>
            {
                persistentStoreOptions.UsePostgres(adoProviderOptions =>
                {
                    adoProviderOptions.ConnectionString = configuration.GetConnectionString("Postgres")!;
                });
                persistentStoreOptions.RetryInterval = TimeSpan.FromSeconds(1);
                persistentStoreOptions.UseProperties = true;
                persistentStoreOptions.UseNewtonsoftJsonSerializer();
            });
        });
        
        services.AddQuartzServer(quartzHostedServiceOptions =>
        {
            quartzHostedServiceOptions.WaitForJobsToComplete = true;
            quartzHostedServiceOptions.AwaitApplicationStarted = true;
        });
        
        return services;
    }
}