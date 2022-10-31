using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkManagementTool.Data;
using WorkManagementTool.Models;

namespace WorkManagementTool.Controllers
{
    public class JournalController : Controller
    {
        private readonly WorkManagementToolContext _context;

        public JournalController(WorkManagementToolContext context)
        {
            _context = context;
        }

        [HttpGet("GetJobs")]
        public async Task<ActionResult<ResponceJobsModel>> GetJobs(RequestJobsModel model)
        {
            try
            {
                var query = _context.Journal.Where(x => x.DeletedDate.HasValue == model.Fillters.IsTrash &&
                    (model.Fillters.DepartmentId.HasValue ? x.DepartmentId == model.Fillters.DepartmentId.Value : true) &&
                    (!string.IsNullOrEmpty(model.Fillters.SerialNumber) ? x.SerialNumber == model.Fillters.SerialNumber : true));
               
                var count = await query.CountAsync();

                var jobs = await query.OrderByDescending(x => x.Id)
                                       .Skip(model.Pagination.Limit * model.Pagination.Page)
                                       .Take(model.Pagination.Limit)
                                       .ToListAsync();

                return new ResponceJobsModel
                {
                    Count = count,
                    Jobs = jobs
                };
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("Add")]
        public async Task<ActionResult> AddJob([FromQuery] AddRequestjob requestjob)
        {
            try
            {
                var journalRow = new Journal()
                {
                    SerialNumber = requestjob.SerialNumber,
                    UserId = requestjob.UserId,
                    JobDate = requestjob.JobDate,
                    WorkLocationId = requestjob.WorkLocationId,
                    DepartmentId = requestjob.DepartmentId,
                    JobTypeId = requestjob.JobTypeId,
                    Notes = requestjob.Notes,
                    CreateDate = requestjob.CreateDate,
                    LastUpdateDate = requestjob.LastUpdateDate,
                    ArchivedDate = requestjob.ArchivedDate,
                    DeletedDate = requestjob.DeletedDate,

                };
                if (journalRow != null)
                {
                    await _context.Journal.AddAsync(journalRow);
                    await _context.SaveChangesAsync();
                    return Ok(await _context.Journal.ToListAsync());
                }
                return BadRequest();
            }
            catch
            {
                return BadRequest();
                throw;
            }
        }

        [HttpPut("Update")]
        public async Task<ActionResult> UpdateJob([FromQuery] AddRequestjob requestjob)
        {
            try
            {
                var query = await _context.Journal.FindAsync(requestjob.Id);
                if (query.ArchivedDate > DateTime.Now.AddDays(-7))
                {
                    return BadRequest();
                }
                if (query == null)
                {
                    return BadRequest();
                }
                else
                {
                    query.UserId = requestjob.UserId;
                    query.JobDate = requestjob.JobDate;
                    query.WorkLocationId = requestjob.WorkLocationId;
                    query.DepartmentId = requestjob.DepartmentId;
                    query.JobTypeId = requestjob.JobTypeId;
                    query.Notes = requestjob.Notes;
                    query.CreateDate = requestjob.CreateDate;
                    query.LastUpdateDate = requestjob.LastUpdateDate;
                    query.ArchivedDate = requestjob.ArchivedDate;
                    query.DeletedDate = requestjob.DeletedDate;


                    await _context.SaveChangesAsync();
                    return Ok(await _context.Journal.ToListAsync());
                }
            }
            catch 
            {
                BadRequest();
                throw;
            }

        }
    }
}
