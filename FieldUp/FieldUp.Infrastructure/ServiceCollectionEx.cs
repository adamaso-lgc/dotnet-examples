using Amazon.SecretsManager;
using FieldUp.Domain.Events;
using FieldUp.Infrastructure.Configurations;
using FieldUp.Infrastructure.Extensions;
using FieldUp.Infrastructure.Persistence;
using FieldUp.Infrastructure.Persistence.Marten;
using FieldUp.Infrastructure.Projections;
using FieldUp.Infrastructure.Services;
using JasperFx;
using JasperFx.Events.Daemon;
using JasperFx.Events.Projections;
using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Wolverine.Marten;

namespace FieldUp.Infrastructure;

public static class ServiceCollectionEx
{
    extension(IServiceCollection services)
    {
        public void AddSecrets(IHostEnvironment env)
        {
            services.AddSingleton<IAmazonSecretsManager>(_ =>
            {
                if (env.IsDevelopment())
                {
                    return new AmazonSecretsManagerClient(new AmazonSecretsManagerConfig
                    {
                        ServiceURL = "http://localhost:4566",
                        UseHttp = true
                    });
                }

                return new AmazonSecretsManagerClient();
            });

            services.AddSingleton<ISecretProvider, AwsSecretProvider>();
        }

        public void AddMartenInfrastructure(IConfiguration configuration)
        {
            services.Configure<PostgresOptions>(options => 
                configuration.GetSection(PostgresOptions.SectionKey).Bind(options));

            services.AddMarten(sp =>
                {
                    var options = sp.GetRequiredService<IOptions<PostgresOptions>>().Value;
                    var secretReader = sp.GetRequiredService<ISecretProvider>();
                
                    var password = secretReader.GetRequiredSecret(PostgresOptions.PasswordSecretKey);
                    var connectionString = options.BuildConnectionString(password);

                    var opts = new StoreOptions();
                    opts.Connection(connectionString);
                    opts.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
                    opts.Projections.Add<ReservationViewProjection>(ProjectionLifecycle.Async);
            
                    return opts;
                })
                .AddAsyncDaemon(DaemonMode.HotCold)
                .IntegrateWithWolverine(cfg =>
                {
                    cfg.UseWolverineManagedEventSubscriptionDistribution = true;
                });

            services.AddScoped(typeof(IRepository<,>), typeof(MartenRepository<,>));
            services.AddScoped(typeof(IEventRepository<,>), typeof(MartenEventRepository<,>));
        }
        
        public void AddEmailService(IConfiguration configuration)
        {
            services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.SectionKey));
            services.AddScoped<IEmailService, EmailService>();
        }
    }
}