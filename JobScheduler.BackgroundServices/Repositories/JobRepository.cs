using JobScheduler.BackgroundServices.Contexts;
using JobScheduler.BackgroundServices.Entities;
using JobScheduler.BackgroundServices.Enums;

namespace JobScheduler.BackgroundServices.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly ApplicationDbContext _context;

        public JobRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void UpdateStatus(Guid id, JobStatus status)
        {
            var entry = new JobEntity { Id = id, Status = status };

            _context.Jobs.Attach(entry);
            _context.Entry(entry)
                .Property(x => x.Status)
                .IsModified = true;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
