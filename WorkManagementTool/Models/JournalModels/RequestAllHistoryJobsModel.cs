using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WorkManagementTool.Models.JournalModels
{
    /// <summary>
    /// This Class <c> RequestAllHistoryJobsModel </c> For Fillter And Request parametrs
    /// </summary>
    public class RequestAllHistoryJobsModel
    {
        public PaginationAllHistroy Pagination { get; set; } = new();
        public JournalFilltersAllHistory Fillters { get; set; } = new();
    }
    /// <summary>
    /// Types of filtering
    /// </summary>
    public class JournalFilltersAllHistory
    {
        [Range(1, 4, ErrorMessage = "Please enter valid Department")]
        public int? DepartmentId { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    /// <summary>
    /// Request to View 
    /// </summary>
    public class PaginationAllHistroy
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