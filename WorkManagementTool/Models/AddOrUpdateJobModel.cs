using System.ComponentModel.DataAnnotations;

namespace WorkManagementTool.Models
{
    public class AddOrUpdateJobModel
    {
        public DateTime? JobDate { get; set; }
        
        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public int? WorkLocationId { get; set; }

        public int? JobTypeId { get; set; }

        [MaxLength(1024)]
        public string? Notes { get; set; }
    }
}
