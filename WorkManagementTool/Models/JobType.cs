using MessagePack;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WorkManagementTool.Models
{
    public class JobType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }    
        public string  Name { get; set; }    
    }
}
