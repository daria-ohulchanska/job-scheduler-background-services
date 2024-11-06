using JobScheduler.BackgroundServices.Contexts;
using JobScheduler.BackgroundServices.Repositories;

namespace JobScheduler.BackgroundServices.Services;

public interface IUnitOfWork
{
    public IJobRepository JobRepository { get; }
    public IJobHistoryRepository JobHistoryRepository { get; }
    public ApplicationDbContext Context { get; }
    public Task SaveAsync();
}