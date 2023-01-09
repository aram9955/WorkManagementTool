using System.ComponentModel.DataAnnotations.Schema;

namespace WorkManagementTool.Data
{
    // <summary>
    /// Job types by Department  Id , DepartmentId And JobType Name .
    /// </summary>
    public class JobType
    {
        /// <summary>
        /// Types id
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// typeId by department
        /// </summary>
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }

        /// <summary>
        /// JobType Name
        /// </summary>
        public string Name { get; set; }
    }
}
