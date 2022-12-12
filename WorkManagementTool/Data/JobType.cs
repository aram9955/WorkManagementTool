using MessagePack;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WorkManagementTool.Data
{
    // <summary>
    /// Job types by Department  Id , DepartmentId And JobType Name .
    /// </summary>
    public class JobType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public string Name { get; set; }
    }
}
