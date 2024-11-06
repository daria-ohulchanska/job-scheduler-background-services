using JobScheduler.BackgroundServices.Enums;

namespace JobScheduler.BackgroundServices.Repositories
{
    public interface IJobRepository
    {
        void UpdateStatus(Guid id, JobStatus status);
        Task SaveAsync();
    }
}
