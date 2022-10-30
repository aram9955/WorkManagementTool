using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using WorkManagementTool.Data;
using WorkManagementTool.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static WorkManagementTool.Models.RequestJobModel;

namespace WorkManagementTool.Controllers
{
    public class JournalController : Controller
    {
        private readonly WorkManagementToolContext _context;
        public JournalController(WorkManagementToolContext context)
        {
            _context = context;
        }

        [HttpGet("All")]
        public async Task<ActionResult<List<Journal>>> GetJobs()
        {
            try
            {
                var query = await _context.Journal.Where(x => x.DeletedDate == null).ToListAsync();
                if (query != null)
                {
                    return Ok(query);
                }
                else
                {
                    NotFound();
                }
            }
            catch
            {
                BadRequest();
                throw;
            }
            return NotFound();
        }



        [HttpGet("Search")]
        public async Task<ActionResult<Journal>> GetJobBy(RequestJobModel requestJob)
        {
            try
            {
                if (!String.IsNullOrEmpty(requestJob.SerialNumber))
                {
                    var query = await _context.Journal.Where(x => x.DeletedDate == null).FirstOrDefaultAsync(x => x.SerialNumber == requestJob.SerialNumber);

                    if (query == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        return Ok(query);
                    }
                }
            }
            catch
            {
                BadRequest("");
                throw;
            }
            return NotFound();
        }



        [HttpGet("JobByDepartment")]
        public async Task<ActionResult<List<Journal>>> GetJobs(RequestByDepartment requestByDepartment, Pagination pagination)
        {
            try
            {
                if (requestByDepartment != null)
                {

                    var query = _context.Journal.Where(x => x.DepartmentId == requestByDepartment.Id).
                       Skip(pagination.Page * pagination.Limit).Take(pagination.Limit).Where(x => x.DeletedDate == null);
                    if (query != null)
                    {
                        return await _context.Journal.ToListAsync();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch
            {
                return BadRequest();
                throw;
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
        [HttpGet("Trash")]
        public async Task<ActionResult<List<Journal>>> GetTrash()
        {
            try
            {
                var query = _context.Journal.Where(x => x.DeletedDate != null).ToListAsync();
                if (query != null)
                {
                    return Ok(await query);
                }
                else
                {
                    NotFound();
                }
            }
            catch
            {
                BadRequest();
                throw;
            }
            return NotFound();
        }
        [HttpGet("TrashDepartment")]
        public async Task<ActionResult<List<Journal>>> GetDepartmentTrash(RequestByDepartment request)
        {
            try
            {
                var query = _context.Journal.Where(x => x.DeletedDate != null).Where(x =>x.DepartmentId == request.Id && x.UserId == request.DeletedBy).ToListAsync();
                if (query != null)
                {
                    return Ok(await query);
                }
                else
                {
                    NotFound();
                }
            }
            catch
            {
                BadRequest();
                throw;
            }
            return NotFound();
        }
    }
}
