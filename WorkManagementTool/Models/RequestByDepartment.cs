namespace WorkManagementTool.Models
{
    public class RequestByDepartment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DepartmentKey { get; set; }
        public Guid DeletedBy { get; set; }
      
    }
    public class Pagination
    {
        public int Page { get; set; }
        public int Limit { get; set; }
    }
}
