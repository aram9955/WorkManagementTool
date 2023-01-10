namespace WorkManagementTool.Models.RequestAnalyserModels
{
    public class AnalyserFillters
    {
        public List<Guid> UserId { get; set; }

        public List<int> WorkLocationId { get; set; }

        public List<int> DepartmentId { get; set; }


        public List<int> JobTypeId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public bool Archived { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public bool IsTrash { get; set; }
    }
}
