using WorkManagementTool.Data;

namespace WorkManagementTool.Models.JournalModels
{
    /// <summary>
    /// This Class <c> ResponseJobsModel </c>For Response jobs and jobs Count . 
    /// </summary>
    public class ResponseJobsModel
    {
        /// <summary>
        /// For Jobs .
        /// </summary>
        public List<Journal> Jobs { get; set; } = new();
        /// <summary>
        /// For Jobs Count . 
        /// </summary>
        public int Count { get; set; }
    }
}
