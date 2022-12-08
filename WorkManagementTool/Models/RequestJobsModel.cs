using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WorkManagementTool.Models
{
    public class RequestJobsModel
    {
        public Pagination Pagination { get; set; } = new();

        public JournalFillters Fillters { get; set; } = new();
    }

    public class JournalFillters
    {
        public DateTime?  JobDate { get; set; }
        public int? DepartmentId { get; set; }
        public Guid? UserId { get; set; }
        public int? WorkLocationId { get; set; }
        public int? JobTypeId { get; set; }
        [Required]
        public bool IsTrash { get; set; }
        public string? SerialNumber { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    


    public class Pagination
    {
        [Required]
        [DefaultValue(1)]
        public int? Page { get; set; } = 1;

        [Required]
        [DefaultValue(10)]
        public int? Limit { get; set; } = 10;
    }
}
