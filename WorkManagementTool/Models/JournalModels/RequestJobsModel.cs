using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WorkManagementTool.Models.JournalModels
{
    /// <summary>
    /// This Class <c> RequestJobsModel </c>For Fillter And Request parametrs
    /// </summary>
    public class RequestJobsModel
    {
        public Pagination Pagination { get; set; } = new();
        public JournalFillters Fillters { get; set; } = new();
    }
    /// <summary>
    /// Types of filtering
    /// </summary>
    public class JournalFillters
    {
        public DateTime? JobDate { get; set; }
        [Range(1, 4, ErrorMessage = "Please enter valid Department")]
        public int? DepartmentId { get; set; }
        public Guid? UserId { get; set; }
        [Range(1, 81, ErrorMessage = "Please enter valid WorkLocation")]
        public int? WorkLocationId { get; set; }
        [Range(1, 34, ErrorMessage = "Please enter valid JobType")]
        public int? JobTypeId { get; set; }
        [Required]
        public bool IsTrash { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    /// <summary>
    /// Request to View 
    /// </summary>
    public class Pagination
    {
        /// <summary>
        /// Page Default = 1.
        /// </summary>
        [Required]
        [DefaultValue(1)]
        public int? Page { get; set; } = 1;
        /// <summary>
        /// Limit default = 10.
        /// </summary>
        [Required]
        [DefaultValue(10)]
        public int? Limit { get; set; } = 10;
    }
}