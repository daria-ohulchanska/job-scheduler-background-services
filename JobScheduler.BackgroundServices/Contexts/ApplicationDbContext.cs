using JobScheduler.BackgroundServices.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobScheduler.BackgroundServices.Contexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { 
    }

    public DbSet<JobEntity> Jobs { get; set; }
    public DbSet<JobHistoryEntity> JobStatusHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}