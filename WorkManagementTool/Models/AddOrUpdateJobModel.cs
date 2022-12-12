using System.ComponentModel.DataAnnotations;
namespace WorkManagementTool.Models
{
    /// <summary>
    /// This class <c> AddOrUpdateJobModel </c> pass parameters to Journal for Add and Update.
    /// </summary>
    public class AddOrUpdateJobModel
    {
        public DateTime? JobDate { get; set; }
        /// <summary>
        /// Start is 1 to 4 . 
        /// </summary>
        [Required]
        [Range(1, 4, ErrorMessage = "Please enter valid Department")]
        public int DepartmentId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        /// <summary>
        /// Start is 1 to 81 . 
        /// </summary>
        /// 
        [Range(1, 81, ErrorMessage = "Please enter valid WorkLocation")]
        public int? WorkLocationId { get; set; }
        /// <summary>
        /// Start is 1 to 34 . 
        /// </summary>
        [Range(1, 31, ErrorMessage = "Please enter valid JobType")]
        public int? JobTypeId { get; set; }
        /// <summary>
        /// Max Lenght 1024 char .
        /// </summary>
        [MaxLength(1024)]
        public string? Notes { get; set; }
    }
}