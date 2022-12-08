using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NuGet.Configuration;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using WorkManagementTool.Data;
using WorkManagementTool.Models;
using WorkManagementTool.Models.Configs;

namespace WorkManagementTool.Controllers
{
    public class JournalController : Controller
    {
        private readonly WorkManagementToolContext _context;
        private readonly JournalConfigs _configs;

        public JournalController(WorkManagementToolContext context, IOptions<JournalConfigs> configs)
        {
            _context = context;
            _configs = configs.Value;
        }
        private static readonly Dictionary<int, int> SerialNumber = new Dictionary<int, int>();
      

        public static string GetSerialNumber()
        {
            int year = DateTime.Today.Year;
            int Id;

            SerialNumber.TryGetValue(year, out Id);

            Id++;
            SerialNumber[year] = Id;

            return $"{year}-{Id}";
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
                    query = query.Where(x => x.SerialNumber == model.Fillters.SerialNumber && model.Fillters.DepartmentId == x.DepartmentId);
                }

                if (model.Fillters.DeletedDate.HasValue)
                {
                    query = query.Where(x => x.DeletedDate == model.Fillters.DeletedDate.Value);
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

                if (model.Fillters.StartDate.HasValue)
                {
                    query = query.Where(x => model.Fillters.StartDate <= x.CreateDate);
                }
                if (model.Fillters.EndDate.HasValue)
                {
                    query = query.Where(x => model.Fillters.EndDate >= x.CreateDate);
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
                if (model.JobDate == null
                    && model.Notes == null
                    && model.WorkLocationId == null
                    && model.JobTypeId == null && model.UserId != null && model.DepartmentId != null)
                {
                    return BadRequest();
                }

                var journalRow = new Journal()
                {
                    SerialNumber = GetSerialNumber(),

                    UserId = model.UserId,
                    DepartmentId = model.DepartmentId,

                    JobDate = model.JobDate,
                    WorkLocationId = model.WorkLocationId,
                    JobTypeId = model.JobTypeId,
                    Notes = model.Notes,

                    CreateDate = DateTime.UtcNow,
                    ArchivedDate = DateTime.UtcNow.AddDays(_configs.ArchivedDate)
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
            if (model.JobDate == null
               && model.Notes == null
               && model.WorkLocationId == null
               && model.JobTypeId == null && model.UserId != null && model.DepartmentId != null)
            {
                return BadRequest();
            }

            var journal = await _context.Journal.FindAsync(id);
            if (journal.DeletedDate.HasValue && journal.Id <= 0)
            {
                return BadRequest();
            }
            if (journal == null)
            {
                return NotFound();
            }


            journal.JobDate = model.JobDate;
            journal.WorkLocationId = model.WorkLocationId;
            journal.JobTypeId = model.JobTypeId;
            journal.Notes = model.Notes;

            journal.LastUpdateDate = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {

                return NotFound();
            }
            
            return NoContent();
        }
       
        [HttpDelete("{id}")]
        public async Task<ActionResult<Journal>> DeletedJob(int id)
        {
            var job = await _context.Journal.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            job.DeletedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("Recover/{id}")]
        public async Task<ActionResult<Journal>> RecoverJob(int id)
        {
            var job = await _context.Journal.FirstOrDefaultAsync(x => x.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            job.DeletedDate = null;

            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
