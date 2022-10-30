using System.ComponentModel.DataAnnotations;

namespace WorkManagementTool.Models
{
    public class RequestJobModel
    {
        [Required]
        [MaxLength(16)]
        public string SerialNumber { get; set; }
    }
}










