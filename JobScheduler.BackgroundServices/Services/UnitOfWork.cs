using JobScheduler.BackgroundServices.Contexts;
using JobScheduler.BackgroundServices.Repositories;

namespace JobScheduler.BackgroundServices.Services;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(ApplicationDbContext context, 
        IJobRepository jobRepository, 
        IJobHistoryRepository jobHistoryRepository)
    {
        Context = context;
        JobRepository = jobRepository;
        JobHistoryRepository = jobHistoryRepository;
    }
        
    public IJobRepository JobRepository { get; }
    public IJobHistoryRepository JobHistoryRepository { get; }
    public ApplicationDbContext Context { get; }

    public async Task SaveAsync()
    {
        await Context.SaveChangesAsync();
    }
}