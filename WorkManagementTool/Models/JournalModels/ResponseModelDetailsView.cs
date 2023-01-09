using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WorkManagementTool.Models.JournalModels
{
    /// <summary>
    /// This CLass <c>ResponseModelDetailsView</c> For Response DetailsView Case
    /// </summary>
    /// 
    [Keyless]
    public class ResponseModelDetailesView
    {
        public string? Field { get; set; }

        public string? PreviusValue { get; set; }

        public string? NewValue { get; set; }
        [DataType(DataType.Date)]
        public DateTime LastUpdateDate { get; set; }

    }

}
