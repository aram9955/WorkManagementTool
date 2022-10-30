using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        public DbSet<WorkLocation> WorkLocation { get; set; }
    }
}
