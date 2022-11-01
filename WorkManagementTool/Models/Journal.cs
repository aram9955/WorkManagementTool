using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkManagementTool.Models
{
    public class Journal
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(16)]
        public string SerialNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime? JobDate { get; set; }

        [ForeignKey("Department")]
        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("WorkLocation")]
        public int? WorkLocationId { get; set; }

        [ForeignKey("JobType")]
        public int? JobTypeId { get; set; }

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

        public Guid? DeletedBy { get; set; }
    }
}
