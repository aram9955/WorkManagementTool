using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkManagementTool.Data
{
    /// <summary>
    /// Class <c> Department </c> fields Id, Name and DepartmentKey
    /// </summary>
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]  
        public string DepartmentKey { get; set; }
    }
}
