namespace WorkManagementTool.Models
{
    public class Journal
    {
        public int Id { get; set; }
        public string? SerialNumber { get; set; }
        public DateTime JobDate { get; set; }   
        public int DepartmentId { get; set; }
        public Guid UserId      { get; set; }
        public int WorkLocationId { get; set; }
        public int JobTypeId     { get; set; }
        public string? Notes     { get; set; }   
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public DateTime DeletedDate { get; set; }
        public DateTime Archived { get; set; }
    }
}
