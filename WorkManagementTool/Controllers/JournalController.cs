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
                var query = _context.Journal.Where(x => x.DeletedDate.HasValue == model.Fillters.IsTrash);

                if (model.Fillters.DepartmentId.HasValue)
                {
                    query = query.Where(x => x.DepartmentId == model.Fillters.DepartmentId.Value);
                }

                if (!string.IsNullOrEmpty(model.Fillters.SerialNumber))
                {
                    query = query.Where(x => x.SerialNumber == model.Fillters.SerialNumber);
                }

                if (model.Fillters.DeletedDate.HasValue)
                {
                    query = query.Where(x => x.DeletedDate == model.Fillters.DeletedDate.Value);
                }

                if (model.Fillters.DeletedBy.HasValue)
                {
                    query = query.Where(x => x.DeletedBy == model.Fillters.DeletedBy.Value);
                }

                if (model.Fillters.JobDate.HasValue)
                {
                    query = query.Where(x => x.JobDate == model.Fillters.JobDate.Value);
                }

                if (model.Fillters.WorkLocationId != null)
                {
                    query = query.Where(x => x.WorkLocationId == model.Fillters.WorkLocationId);
                }

                if (model.Fillters.JobTypeId != null)
                {
                    query = query.Where(x => x.JobTypeId == model.Fillters.JobTypeId);
                }


                var count = await query.CountAsync();

                var jobs = await query.OrderByDescending(x => x.Id)
                                       .Skip(model.Pagination.Limit!.Value * (model.Pagination.Page!.Value - 1))
                                       .Take(model.Pagination.Limit!.Value)
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

        [HttpGet("Get/{id}")]
        public async Task<ActionResult<Journal>> GetJob(int id)
        {
            var journal = await _context.Journal.FindAsync(id);

            if (journal == null)
            {
                return NotFound();
            }

            return Ok(journal);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Journal>> AddJob(AddOrUpdateJobModel model)
        {
            try
            {
                if (model == null) 
                {
                    return BadRequest();
                }

                var journalRow = new Journal()
                {
                    SerialNumber = "2022-14", // to do
                    
                    UserId = model.UserId,
                    DepartmentId = model.DepartmentId,

                    JobDate = model.JobDate,
                    WorkLocationId = model.WorkLocationId,
                    JobTypeId = model.JobTypeId,
                    Notes = model.Notes,

                    CreateDate = DateTime.UtcNow,
                    ArchivedDate = DateTime.UtcNow.AddDays(7) // move config
                };

                await _context.Journal.AddAsync(journalRow);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetJob), new { id = journalRow.Id }, (journalRow));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult<Journal>> PutJob(int id, AddOrUpdateJobModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var journal = await _context.Journal.FindAsync(id);
            if (journal == null)
            {
                return NotFound();
            }

            journal.JobDate = model.JobDate;
            journal.WorkLocationId = model.WorkLocationId;
            journal.JobTypeId = model.JobTypeId;
            journal.Notes = model.Notes;

            journal.LastUpdateDate = DateTime.UtcNow;

            _context.Journal.Update(journal);
            await _context.SaveChangesAsync();

            return Ok(journal);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<Journal>> DeletedJob(int id, DeleteJobModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var job = await _context.Journal.FindAsync(id); 
            if (job == null)
            {
                return NotFound();
            }

            job.DeletedBy = model.DeletedBy;
            job.DeletedDate = DateTime.UtcNow;

            _context.Journal.Update(job);
            await _context.SaveChangesAsync();

            return Ok(job);

        }
    }
}
