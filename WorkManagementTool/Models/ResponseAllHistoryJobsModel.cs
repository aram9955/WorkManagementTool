using WorkManagementTool.Data;
namespace WorkManagementTool.Models
{
    /// <summary>
    /// This Class <c> ResponseAllHistoryJobsModel </c>For Responce History jobs  and History jobs Count . 
    /// </summary>
    public class ResponseAllHistoryJobsModel
    {
        ////// <summary>
        /// For History Jobs .
        /// </summary>
        public List<JournalAllHistory> Jobs { get; set; } = new();
        /// <summary>
        /// For History Jobs Count .
        /// </summary>
        public int Count { get; set; } = new();
    }
}