using JobScheduler.BackgroundServices.Configurations;
using JobScheduler.BackgroundServices.Entities;
using JobScheduler.BackgroundServices.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace JobScheduler.BackgroundServices.Services;

public class JobStatusProcessor : RabbitMqConsumer
{
    private readonly IServiceScopeFactory _scopeFactory;

    public JobStatusProcessor(
        IServiceScopeFactory scopeFactory,
        IConnection connection,
        IOptions<RabbitMqSettings> settings) 
        : base(connection, settings)
    {
        _scopeFactory = scopeFactory;
    }

    public override async Task ProcessMessage(string message)
    {
        var jobHistory = JsonConvert.DeserializeObject<JobHistoryEntity>(message);
        if (jobHistory == null)
            return;

        using var scope = _scopeFactory.CreateScope();

        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        await using var transaction = await unitOfWork.Context.Database.BeginTransactionAsync();

        jobHistory.TransactionId = transaction.TransactionId;

        unitOfWork.JobHistoryRepository.Add(jobHistory);
        unitOfWork.JobRepository.UpdateStatus(jobHistory.JobId, jobHistory.Status);

        await unitOfWork.SaveAsync();
        await transaction.CommitAsync();
    }
}