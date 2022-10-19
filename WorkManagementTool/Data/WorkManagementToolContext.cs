using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkManagementTool.Models;

namespace WorkManagementTool.Data
{
    public class WorkManagementToolContext : DbContext
    {
        public WorkManagementToolContext(DbContextOptions<WorkManagementToolContext> options)
            : base(options) => Database.EnsureCreated();

        public DbSet<WorkManagementTool.Models.Department> Departments { get; set; }
        public DbSet<WorkManagementTool.Models.JobType> JobTypes { get; set; }
        public DbSet<WorkManagementTool.Models.Journal> Journals { get; set; }  
        public DbSet<WorkManagementTool.Models.WorkLocation> WorkLocation  { get; set; }   
    }
}
