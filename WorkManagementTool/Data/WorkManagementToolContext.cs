using Microsoft.EntityFrameworkCore;
using WorkManagementTool.Models;

namespace WorkManagementTool.Data
{
    public class WorkManagementToolContext : DbContext
    {
        public WorkManagementToolContext(DbContextOptions<WorkManagementToolContext> options)
            : base(options) => Database.EnsureCreated();

        public DbSet<Department> Departments { get; set; }
        public DbSet<JobType> JobTypes { get; set; }
        public DbSet<Journal> Journal { get; set; }  
        public DbSet<WorkLocation> WorkLocation  { get; set; }   
    }
}
