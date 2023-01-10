using Microsoft.Build.Framework;
using System.ComponentModel;

namespace WorkManagementTool.Models
{

    /// <summary>
    /// This Class <c>RequestModelAnalyserDepartmentId</c> For Filtring Request in Analytics
    /// </summary>
    public class RequestModelAnalyserDepartmentId
    {

        /// <summary>
        /// this field <para>List<int><para> for JobTypes
        /// </summary>     
        public List<int> DepartmentId { get; set; } = new();

        /// <summary>
        /// filtring by StartDate
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// filtring by EndDate
        /// </summary>
        public DateTime? EndDate { get; set; }

        [Required]
        public bool Archived { get; set; }
        [Required]
        public bool IsTrash { get; set; }
    }
}