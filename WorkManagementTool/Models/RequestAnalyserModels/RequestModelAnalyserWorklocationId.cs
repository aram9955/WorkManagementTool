using Microsoft.Build.Framework;
using System.ComponentModel;

namespace WorkManagementTool.Models
{

    /// <summary>
    /// This Class <c>RequestModelAnalyserWorklocationId</c> For Filtring Request in Analytics
    /// </summary>
    public class RequestModelAnalyserWorklocationId
    {
        /// <summary>
        /// this field <para>List<int><para>
        /// <summary>
        public List<int> WorkLocationId { get; set; } = new();

        /// <summary>
        /// For Filtring by StartDate
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// For Filtring by EndDate
        /// </summary>
        public DateTime? EndDate { get; set; }

        [Required]
        public bool Archived { get; set; }
        [Required]
        public bool IsTrash { get; set; }
    }
}