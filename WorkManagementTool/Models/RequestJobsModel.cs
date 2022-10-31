namespace WorkManagementTool.Models
{
    public class RequestJobsModel
    {
        public Pagination Pagination { get; set; } = new();

        public JournalFillters Fillters { get; set; } = new();
    }

    public class JournalFillters
    {
        public bool IsTrash { get; set; }

        public int? DepartmentId { get; set; }

        public string SerialNumber { get; set; }
    }

    public class Pagination
    {
        public int Page { get; set; }
        public int Limit { get; set; }
    }
}
