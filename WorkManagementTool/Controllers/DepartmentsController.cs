using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkManagementTool.Data;
using WorkManagementTool.Models;

namespace WorkManagementTool.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly WorkManagementToolContext _context;

        public DepartmentsController(WorkManagementToolContext context)
        {
            _context = context;
        }

        [HttpGet("GetDepartmentByKey")]
        public  async Task<ActionResult<Department>> GetDepartment(string departmentKey)
        {
            if (!string.IsNullOrEmpty(departmentKey))
            {
                var department = await _context.Departments.FirstOrDefaultAsync(x => departmentKey.StartsWith(x.DepartmentKey));

                if (department == null) {
                    return NotFound();
                }

                return Ok(department);
            }

            return BadRequest("Department key is required."); 
        }

        [HttpGet("GetJobTypes")]
        public async Task<ActionResult<List<JobType>>> GetJobTypes(int departmentId)
        {
            var jobTypes = await _context.JobTypes.Where(x => x.DepartmentId == departmentId).ToListAsync();
            if (jobTypes?.Count > 0)
            {
                return jobTypes;
            }

            return NotFound();
        }

        [HttpGet("GetWorkLocations")]
        public async Task<List<WorkLocation>> GetWorkLocation() => await _context.WorkLocation.ToListAsync();
    }
}
