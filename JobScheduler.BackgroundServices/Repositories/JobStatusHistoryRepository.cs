using JobScheduler.BackgroundServices.Contexts;
using JobScheduler.BackgroundServices.Entities;

namespace JobScheduler.BackgroundServices.Repositories
{
    public class JobStatusHistoryRepository : IJobHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public JobStatusHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(JobHistoryEntity entity)
        {
            _context.JobStatusHistory.Add(entity);
        }
        
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
