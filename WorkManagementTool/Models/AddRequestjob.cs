using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkManagementTool.Models
{
    public class AddRequestjob
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(16)]
        public string SerialNumber { get; set; }
        [DataType(DataType.Date)]
        public DateTime? JobDate { get; set; }
        public int DepartmentId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public int WorkLocationId { get; set; }
        public int JobTypeId { get; set; }
        [MaxLength(1024)]
        public string? Notes { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? LastUpdateDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DeletedDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime ArchivedDate { get; set; }

    }
}
