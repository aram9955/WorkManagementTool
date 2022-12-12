using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkManagementTool.Data;
using WorkManagementTool.Models;
using Microsoft.Extensions.Options;
using WorkManagementTool.Models.Configs;

namespace WorkManagementTool.Controllers
{
    /// <summary>
    /// Controller for CRUD And History view  
    /// </summary>
    public class JournalController : Controller
    {
        /// <summary>
        /// For Dbsets
        /// </summary>
        private readonly WorkManagementToolContext _context;
        /// <summary>
        /// For ArchivedDate
        /// </summary>
        private readonly JournalConfigs _configs;

        /// <summary>
        ///  _context and _configs.Value added for Db set and Configuration class field ArchivedDate
        /// </summary>
        /// <param name="context"></param>
        /// <param name="configs"></param>
        public JournalController(WorkManagementToolContext context, IOptions<JournalConfigs> configs)
        {
            _context = context;
            _configs = configs.Value;
        }
        /// <summary>
        /// Generation for SerialNumber . 
        /// </summary>
        private static readonly Dictionary<int, int> SerialNumber = new Dictionary<int, int>();

        /// <summary>
        /// This Method <c> GetSerialNumber </c> For SerialNumber temporary Value.
        /// </summary>
        /// <returns> SerialNumber  </returns>
        public static string GetSerialNumber()
        {
            int year = DateTime.Today.Year;
            int Id;
            SerialNumber.TryGetValue(year, out Id);
            Id++;
            SerialNumber[year] = Id;
            return $"{year}-{Id}";
        }
        /// <summary>
        /// For Get Jobs in sort 
        /// </summary>
        /// <param name="model"> This model .  </param>
        /// <returns> Jobs in Filtred data . </returns>
        [HttpGet("GetJobs")]
        public async Task<ActionResult<ResponseJobsModel>> GetJobs(RequestJobsModel model)
        {
            try
            {
                if (model.Fillters.DepartmentId <= 0 || model.Fillters.DepartmentId > 4)
                {
                    return BadRequest(ModelState);
                }

                if (model.Fillters.WorkLocationId <= 0 || model.Fillters.WorkLocationId > 81)
                {
                    return BadRequest(ModelState);
                }
                if (model.Fillters.JobTypeId <= 0 || model.Fillters.JobTypeId > 31)
                {
                    return BadRequest(ModelState);
                }

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

                return new ResponseJobsModel
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

        /// <summary>
        /// This Method <c> GetJob </c> For Returned one job By Id .
        /// </summary>
        /// Return by Id .
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// This Method <c> AddJob </c> For Create New Job .
        /// </summary>
        /// <param name="model"></param>
        /// <returns>In Jornal table Created new row . </returns>
        [HttpPost("Create")]
        public async Task<ActionResult<Journal>> AddJob(AddOrUpdateJobModel model)
        {

            try
            {

                if (model.JobTypeId == null && model.JobDate == null
                    && model.Notes == null && model.WorkLocationId == null)
                {
                    return BadRequest();
                }


                var journalRow = new Journal()
                {
                    SerialNumber = GetSerialNumber(),
                    UserId = model.UserId,
                    DepartmentId = model.DepartmentId,
                    JobStatus = "Created",
                    JobDate = model.JobDate,
                    WorkLocationId = model.WorkLocationId,
                    JobTypeId = model.JobTypeId,
                    Notes = model.Notes,
                    CreateDate = DateTime.UtcNow,
                    ArchivedDate = DateTime.UtcNow.AddDays(_configs.ArchivedDate)
                };
              
                await _context.Journal.AddAsync(journalRow);
                await _context.SaveChangesAsync();
                var historyProcedure = _context.JournalAllHistory.
                    FromSqlRaw<JournalAllHistory>($"  INSERT INTO JournalAllHistory(JournalId,SerialNumber,JobDate," +
                    $" DepartmentId,UserId ,WorkLocationId ,JobTypeId ,Notes ,CreateDate ,LastUpdateDate ," +
                    $"DeletedDate, ArchivedDate , JobStatus)" +
                    $" SELECT *  FROM Journal WHERE Id = {journalRow.Id}").ToListAsync();
                await _context.SaveChangesAsync();


                return CreatedAtAction(nameof(GetJob), new { id = journalRow.Id }, (journalRow));
            }
            catch
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// This Method <c> PutJob </c> For Update Job .
        /// </summary>
        /// <param name="id">  Update by Id </param>
        /// <param name="model">  </param>
        /// <returns>In Jornal table update row By Id .</returns>
        [HttpPut("Update/{id}")]
        public async Task<ActionResult<Journal>> PutJob(int id, AddOrUpdateJobModel model)
        {
            try
            {
                DateTime updateDate = DateTime.UtcNow;

                if (model.JobTypeId == null && model.JobDate == null
                    && model.Notes == null && model.WorkLocationId == null)
                {
                    return BadRequest();
                }


                var journal = await _context.Journal.FindAsync(id);
                if (journal == null)
                {
                    return NotFound();
                }
                if (updateDate >= journal.ArchivedDate)
                {
                    return BadRequest();
                }

                journal.JobStatus = "Updated";
                journal.JobDate = model.JobDate;
                journal.WorkLocationId = model.WorkLocationId;
                journal.JobTypeId = model.JobTypeId;
                journal.Notes = model.Notes;

                journal.LastUpdateDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                var historyProcedure = _context.JournalAllHistory.
                   FromSqlRaw<JournalAllHistory>($"  INSERT INTO JournalAllHistory(JournalId,SerialNumber,JobDate," +
                   $" DepartmentId,UserId ,WorkLocationId ,JobTypeId ,Notes ,CreateDate ,LastUpdateDate ," +
                   $"DeletedDate, ArchivedDate , JobStatus)" +
                   $" SELECT *  FROM Journal WHERE Id = {journal.Id}").ToListAsync();
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        /// <summary>
        /// This Method <c> DeletedJob </c> For Deleted Job .
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Add DeleteDate in Journal Id row</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Journal>> DeletedJob(int id)
        {
            DateTime updateDate = DateTime.UtcNow;
            var job = await _context.Journal.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            if (updateDate >= job.ArchivedDate)
            {
                return BadRequest();
            }
            job.JobStatus = "Deleted";
            job.DeletedDate = DateTime.UtcNow;

          
            await _context.SaveChangesAsync();

            var historyProcedure = _context.JournalAllHistory.
                    FromSqlRaw<JournalAllHistory>($"  INSERT INTO JournalAllHistory(JournalId,SerialNumber,JobDate," +
                    $" DepartmentId,UserId ,WorkLocationId ,JobTypeId ,Notes ,CreateDate ,LastUpdateDate ," +
                    $"DeletedDate, ArchivedDate , JobStatus)" +
                    $" SELECT *  FROM Journal WHERE Id = {id}").ToListAsync();
            await _context.SaveChangesAsync();


            return NoContent();
        }

        /// <summary>
        /// This Method <c> RecoverJob </c> For Recovery Deleted Job .
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Recovery in Journal row by Id </returns>
        /// 
        [HttpPut("Recover/{id}")]
        public async Task<ActionResult<Journal>> RecoverJob(int id)
        {
            DateTime updateDate = DateTime.UtcNow;
            var job = await _context.Journal.FirstOrDefaultAsync(x => x.Id == id);
            if (job == null)
            {
                return NotFound();
            }
            if (updateDate >= job.ArchivedDate)
            {
                return BadRequest();
            }
            job.JobStatus = "Recovery";
            job.DeletedDate = null;

            await _context.SaveChangesAsync();

            var historyProcedure = _context.JournalAllHistory.
                   FromSqlRaw<JournalAllHistory>($"  INSERT INTO JournalAllHistory(JournalId,SerialNumber,JobDate," +
                   $" DepartmentId,UserId ,WorkLocationId ,JobTypeId ,Notes ,CreateDate ,LastUpdateDate ," +
                   $"DeletedDate, ArchivedDate , JobStatus)" +
                   $" SELECT *  FROM Journal WHERE Id = {id}").ToListAsync();
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// This Method <c> GetDetailedView </c> For History by Detailed .
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Detailed View For All changes in this Journal row by Id </returns>
        /// 
        [HttpGet("Get Detailed view/{id}")]
        public async Task<ActionResult<Journal>> GetDetailedView(int id)
        {
            try
            {
                var journal = await _context.JournalAllHistory
                .OrderByDescending(x => x.Id == id).
                Where(x => x.JournalId == id).ToListAsync();

                if (journal == null)
                {
                    return NotFound();
                }

                return Ok(journal);
            }
            catch (Exception)
            {
                throw;
            }

        }
        /// <summary>
        /// This Method <c> GetHistoryJobs </c> For History by Filtring .
        /// </summary>
        /// <param name="model"> </param>
        /// <returns> Retun All History In Filltring </returns>
        /// 
        [HttpGet("GetAllHistoryJobs")]
        public async Task<ActionResult<ResponseAllHistoryJobsModel>> GetHistoryJobs(RequestAllHistoryJobsModel model)
        {
            try
            {
                if (model != null)
                {

                    var query = _context.JournalAllHistory.Where(x =>
                    (model.Fillters.DepartmentId != null ? model.Fillters.DepartmentId.Value == x.DepartmentId : true)
                    && (!string.IsNullOrEmpty(model.Fillters.SerialNumber) ? model.Fillters.SerialNumber == x.SerialNumber : true));

                    if (model.Fillters.StartDate != null && model.Fillters.EndDate != null)
                    {
                        query = query.Where(x =>
                            model.Fillters.StartDate <= x.CreateDate
                            && model.Fillters.EndDate >= x.CreateDate);
                    }

                    var count = await query.CountAsync();

                    var jobs = await query.OrderByDescending(x => x.Id)
                                           .Skip(model.Pagination.Limit!.Value * (model.Pagination.Page!.Value - 1))
                                           .Take(model.Pagination.Limit!.Value)
                                           .ToListAsync();

                    return new ResponseAllHistoryJobsModel
                    {
                        Count = count,
                        Jobs = jobs
                    };
                }
                return NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}