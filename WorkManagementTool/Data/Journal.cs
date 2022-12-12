using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkManagementTool.Data
{
    /// <summary>
    /// This Class for Create Update Delete and Recovery for Database table .
    /// </summary>
    public class Journal
    {
        /// <summary>
        /// <param name="Id"> Primary Key Id </param>
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// <param name="SerialNumber"> this param Unique </param>
        /// </summary>
        [Required]
        [MaxLength(16)]
        public string? SerialNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime? JobDate { get; set; }
        /// <summary>
        /// DepartmentId Foreign key in Department table Id .
        /// </summary>
        [ForeignKey("Department")]
        [Required]
        public int DepartmentId { get; set; }
        /// <summary>
        /// <param name=" UserId "> This param Is UniqueIdentifier
        /// <summary>

        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// WorkLocationId Foreign key in WorkLocation table Id .
        /// </summary>
        [ForeignKey("WorkLocation")]
        public int? WorkLocationId { get; set; }
        /// <summary>
        /// JobTypeId Foreign key in JobTypes table Id .
        /// </summary>
        [ForeignKey("JobType")]
        public int? JobTypeId { get; set; }

        /// <summary>
        /// Max Lenght 1024 char .
        /// </summary>
        [MaxLength(1024)]
        public string? Notes { get; set; }

        public string JobStatus { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? CreateDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? LastUpdateDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DeletedDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ArchivedDate { get; set; }
    }
}