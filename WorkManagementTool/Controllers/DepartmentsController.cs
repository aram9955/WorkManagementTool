using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkManagementTool.Data;
using WorkManagementTool.Models;

namespace WorkManagementTool.Controllers
{
    /// <summary>
    ///  Whis Class <c> DepartmentsController </c> for (Department , GetJobTypes , GetWorkLocation .
    /// </summary>
    public class DepartmentsController : Controller
    {
        private readonly WorkManagementToolContext _context;

        /// <summary>
        /// For Department Set .
        /// </summary>
        /// <param name="context"></param>
        public DepartmentsController(WorkManagementToolContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Get Department in departmentKey .
        /// </summary>
        /// <param name="departmentKey"></param>
        /// <returns> Return User Department  By DepartmentKey  value (1 - 4) . </returns>
        [HttpGet("GetDepartmentByKey")]
        public async Task<ActionResult<Department>> GetDepartment(string departmentKey)
        {
            if (!string.IsNullOrEmpty(departmentKey))
            {
                var department = await _context.Departments.FirstOrDefaultAsync(x => departmentKey.StartsWith(x.DepartmentKey!));
                if (department == null)
                {
                    return NotFound();
                }

                return Ok(department);
            }
            return BadRequest("Department key is required.");
        }
        /// <summary>
        /// Get JobTypes By DepartmentId
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns> User JobTypes By DepartmentId . </returns>
        [HttpGet("GetJobTypes")]
        public async Task<ActionResult<List<JobType>>> GetJobTypes(int departmentId)
        {
            try
            {
                var jobTypes = await _context.JobTypes.Where(x => x.DepartmentId == departmentId).ToListAsync();
                if (jobTypes?.Count > 0 )
                {
                    return jobTypes;
                }
                return NotFound();
            }
            catch
            {
                BadRequest();
                throw;
            }
        }

        /// <summary>
        /// WorkLocation for All User
        /// </summary>
        /// <returns> Return All WorkLocation For User . </returns>
        [HttpGet("GetWorkLocations")]
        public async Task<List<WorkLocation>> GetWorkLocation() => await _context.WorkLocation.ToListAsync();
    }
}