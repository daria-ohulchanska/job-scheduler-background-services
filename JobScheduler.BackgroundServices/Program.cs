using JobScheduler.BackgroundServices.Configurations;
using JobScheduler.BackgroundServices.Contexts;
using JobScheduler.BackgroundServices.Repositories;
using JobScheduler.BackgroundServices.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                var connectionString = context.Configuration.GetConnectionString("JobSchedulerDb");

                services.AddDbContext<ApplicationDbContext>(options => 
                    options.UseNpgsql(connectionString));
                
                var queueSettings = context.Configuration.GetSection("RabbitMQ").Get<RabbitMqSettings>();

                services.AddSingleton<IConnection>(sp =>
                {
                    var factory = new ConnectionFactory
                    {
                        HostName = queueSettings.HostName,
                        UserName = queueSettings.UserName,
                        Password = queueSettings.Password
                    };
            
                    return factory.CreateConnection();
                });
                
                services.Configure<RabbitMqSettings>(context.Configuration.GetSection("RabbitMQ"));

                services.AddHostedService<JobStatusProcessor>();
                
                services.AddScoped(typeof(IJobRepository), typeof(JobRepository));
                services.AddScoped(typeof(IJobHistoryRepository), typeof(JobStatusHistoryRepository));
                services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));            })
            .Build();

        await host.RunAsync();
    }
}