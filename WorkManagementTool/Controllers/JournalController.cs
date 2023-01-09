using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkManagementTool.Data;
using Microsoft.Extensions.Options;
using WorkManagementTool.Models.Configs;
using WorkManagementTool.Models.JournalModels;

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
        //private static readonly Dictionary<int, int> SerialNumber = new Dictionary<int, int>();

        /// <summary>
        /// This Method <c> GetSerialNumber </c> For SerialNumber temporary Value.
        /// </summary>
        /// <returns> SerialNumber  </returns>
        public async Task<string> GetSerialNumber()
        {
            int number = 1;
            string lastNumber = string.Empty;

            var job = await _context.Journal.Where(x => x.SerialNumber != null).OrderByDescending(x => x.CreateDate).FirstOrDefaultAsync();
            if (job != null)
            {
                lastNumber = job.SerialNumber;
            }

            if (!String.IsNullOrEmpty(lastNumber))
            {
                if (!(DateTime.UtcNow.Year > int.Parse(lastNumber.Split('-')[0])))
                {
                    number = int.Parse(lastNumber.Split('-')[1]);
                    number++;
                }

            }
            return $"{DateTime.UtcNow.Year}-{number}";

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
                if (model.Fillters.JobTypeId <= 0 || model.Fillters.JobTypeId > 34)
                {
                    return BadRequest(ModelState);
                }

                var query = _context.Journal.Where(x => x.DeletedDate.HasValue == model.Fillters.IsTrash);

                if (model.Fillters.DepartmentId.HasValue)
                {
                    query = query.Where(x => x.DepartmentId == model.Fillters.DepartmentId.Value);
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

                string SerialNumber = await GetSerialNumber();
                int number = int.Parse(SerialNumber.Split('-')[1]);

                var saved = false;
                while (!saved)
                {
                    try
                    {
                        journalRow.SerialNumber = SerialNumber;
                        saved = true;
                        await _context.Journal.AddAsync(journalRow);
                        await _context.SaveChangesAsync();

                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException?.Message.Contains("uidx_pid") == true)
                            SerialNumber = $"{DateTime.Now.Year}-{++number}";
                        continue;

                        throw ex;
                    }
                }
                var journalHistory = new JournalAllHistory();

                journalHistory.JournalId = journalRow.Id;
                journalHistory.UserId = journalRow.UserId;
                journalHistory.SerialNumber = journalRow.SerialNumber;
                journalHistory.DepartmentId = journalRow.DepartmentId;
                journalHistory.JobStatus = journalRow.JobStatus;
                journalHistory.JobDate = journalRow.JobDate;
                journalHistory.WorkLocationId = journalRow.WorkLocationId;
                journalHistory.Notes = journalRow.Notes;
                journalHistory.CreateDate = (DateTime)journalRow.CreateDate;
                journalHistory.ArchivedDate = journalRow.ArchivedDate;
                journalHistory.DeletedDate = journalRow.DeletedDate;

                await _context.JournalAllHistory.AddAsync(journalHistory);
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

                if (model.JobDate != null)
                {
                    journal.JobDate = model.JobDate;
                }

                if (model.WorkLocationId != null)
                {
                    journal.WorkLocationId = model.WorkLocationId;
                }

                if (model.JobTypeId != null)
                {
                    journal.JobTypeId = model.JobTypeId;
                }

                if (model.Notes != null)
                {
                    journal.Notes = model.Notes;
                }

                journal.JobStatus = "Updated";
                journal.LastUpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var journalHistory = new JournalAllHistory();

                journalHistory.JournalId = journal.Id;
                journalHistory.UserId = journal.UserId;
                journalHistory.SerialNumber = journal.SerialNumber;
                journalHistory.DepartmentId = journal.DepartmentId;
                journalHistory.JobStatus = journal.JobStatus;
                journalHistory.JobDate = journal.JobDate;
                journalHistory.WorkLocationId = journal.WorkLocationId;
                journalHistory.Notes = journal.Notes;
                journalHistory.CreateDate = (DateTime)journal.CreateDate;
                journalHistory.ArchivedDate = journal.ArchivedDate;
                journalHistory.DeletedDate = journal.DeletedDate;
                journalHistory.LastUpdateDate = journal.LastUpdateDate;

                await _context.JournalAllHistory.AddAsync(journalHistory);
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
            job.LastUpdateDate = DateTime.UtcNow;


            await _context.SaveChangesAsync();

            var journalHistory = new JournalAllHistory();


            journalHistory.JournalId = job.Id;
            journalHistory.UserId = job.UserId;
            journalHistory.SerialNumber = job.SerialNumber;
            journalHistory.DepartmentId = job.DepartmentId;
            journalHistory.JobStatus = job.JobStatus;
            journalHistory.JobDate = job.JobDate;
            journalHistory.WorkLocationId = job.WorkLocationId;
            journalHistory.Notes = job.Notes;
            journalHistory.CreateDate = (DateTime)job.CreateDate;
            journalHistory.ArchivedDate = job.ArchivedDate;
            journalHistory.LastUpdateDate = (DateTime)job.LastUpdateDate;
            journalHistory.DeletedDate = job.DeletedDate;

            await  _context.JournalAllHistory.AddAsync(journalHistory);
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
            if (job.DeletedDate == null)
            {
                return BadRequest();
            }
            job.JobStatus = "Recovery";
            job.DeletedDate = null;
            job.LastUpdateDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var journalHistory = new JournalAllHistory();

            journalHistory.JournalId = job.Id;
            journalHistory.UserId = job.UserId;
            journalHistory.SerialNumber = job.SerialNumber;
            journalHistory.DepartmentId = job.DepartmentId;
            journalHistory.JobStatus = job.JobStatus;
            journalHistory.JobDate = job.JobDate;
            journalHistory.WorkLocationId = job.WorkLocationId;
            journalHistory.Notes = job.Notes;
            journalHistory.CreateDate = (DateTime)job.CreateDate;
            journalHistory.ArchivedDate = job.ArchivedDate;
            journalHistory.LastUpdateDate = (DateTime)job.LastUpdateDate;
            journalHistory.DeletedDate = job.DeletedDate;

            await _context.JournalAllHistory.AddAsync(journalHistory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// This Method <c> GetDetailedView </c> For History by Detailed .
        /// </summary>
        /// <returns> Detailed View For All changes in this Journal row by Id </returns>
        /// 
        [HttpGet("Get Detailed view/{id}")]
        public async Task<ActionResult<ResponseDetailesView>> GetDetailedView(int id)
        {
            try
            {
                if (id < 0 || id > int.MaxValue)
                {
                    return BadRequest();
                }
                else
                {
                    var job = await _context.JournalAllHistory.Where(x => x.JournalId == id).FirstAsync();
                    if (job == null)
                    {
                        return NotFound();
                    }
                }

                var jobDetailed = await _context.ResponseModelDetailesViews
                    .FromSqlRaw($" Exec DetectJobChanges @JobId = {id}").ToListAsync();

                return new ResponseDetailesView()
                {
                    detailesViews = jobDetailed
                };
            }
            catch (Exception)
            {
                return BadRequest();
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
                var query = _context.JournalAllHistory.Where(x =>
                (model.Fillters.DepartmentId != null ? model.Fillters.DepartmentId.Value == x.DepartmentId : true));
        

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

                return new ResponseAllHistoryJobsModel()
                {
                    Jobs = jobs,
                    Count = count,
                };
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}