using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkManagementTool.Data
{
    /// <summary>
    /// Class <c> Department </c> fields Id, Name and DepartmentKey
    /// </summary>
    public class Department
    {
        /// <summary>
        /// This Deparment Id .
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// This Department Name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// This Key for verification .
        /// </summary>
        [Required]  
        public string DepartmentKey { get; set; }
    }
}
