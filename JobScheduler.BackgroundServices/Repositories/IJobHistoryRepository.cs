using JobScheduler.BackgroundServices.Entities;

namespace JobScheduler.BackgroundServices.Repositories
{
    public interface IJobHistoryRepository
    {
        void Add(JobHistoryEntity entity);
        Task SaveAsync();
    }
}
