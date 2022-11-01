using Microsoft.Build.Framework;

namespace WorkManagementTool.Models
{
    public class DeleteJobModel
    {
        [Required]
        public Guid DeletedBy { get; set; }
    }
}
