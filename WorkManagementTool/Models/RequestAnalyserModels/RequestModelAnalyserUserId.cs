using Microsoft.Build.Framework;
using System.ComponentModel;

namespace WorkManagementTool.Models
{

    /// <summary>
    /// This Class <c>RequestModelAnalyserUserId</c> For Filtring Request in Analytics
    /// </summary>
    public class RequestModelAnalyserUserId
    {
        /// <summary>
        /// this field <para>List<int><para>
        /// <summary>
        public List<Guid> UserId { get; set; } = new();

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