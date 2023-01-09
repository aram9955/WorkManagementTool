using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkManagementTool.Data;
using WorkManagementTool.Models;
using WorkManagementTool.Models.ResponseAnalyzerModels;

namespace WorkManagementTool.Controllers
{
    public class AnalyserController : Controller
    {
        public List<double> AnalyserByPercent { get; set; } = new();

        /// <summary>
        /// This Field constant precent 100%;
        /// </summary>
        public const double percent = 100;
        /// <summary>
        /// For sum the count of analytic.
        /// </summary>
        public List<double> AllCount { get; set; } = new();

        private readonly WorkManagementToolContext _context;

        public AnalyserController(WorkManagementToolContext context)
        {
            _context = context;
        }
        /// <summary>
        /// This <c>AnalyticsAsyncUserId </c> for the analytics by UserId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("AnalyticsByUsers")]
        public async Task<ActionResult<ResponseAnalyserModelJobTypes>> AnalyticsAsyncUserId(RequestModelAnalyserUserId model)
        {
            try
            {
                if (model.Equals(null) && model.UserId.Count == 0)
                {
                    return BadRequest();
                }

                if (model.UserId.Count == 1)
                {
                    var allCount = await _context.Journal.Where(x => x.UserId != null && x.DeletedDate.HasValue == model.IsTrash).CountAsync();

                    foreach (var item in model.UserId)
                    {
                        var query = _context.Journal.Where(x => x.UserId == item);
                        query = query.Where(x => x.DeletedDate.HasValue == model.IsTrash);

                        if (model.Archived.Equals(false))
                        {
                            query = query.Where(x => x.ArchivedDate >= DateTime.UtcNow);
                        }
                        if (model.IsTrash.Equals(false))
                        {
                            query = query.Where(x => x.DeletedDate == null);
                        }

                        double count = await query.CountAsync();
                        double percentformula = percent / allCount;

                        return Ok(percentformula * count);
                    }
                }
                if (model.UserId.Count > 1)
                {
                    foreach (var item in model.UserId)
                    {
                        var query = _context.Journal.Where(x => x.UserId == item);

                        if (model.Archived.Equals(false))
                        {
                            query = query.Where(x => x.ArchivedDate >= DateTime.UtcNow);
                        }
                        if (model.IsTrash.Equals(false))
                        {
                            query = query.Where(x => x.DeletedDate == null);
                        }
                        var count = query.CountAsync();

                        if (count != null && count.Result != 0)
                            AllCount.Add(await count);
                        else continue;


                    }
                    var sum = AllCount.Sum();
                    var onepercent = percent / sum;
                    foreach (var item in AllCount)
                    {
                        var queryPrcent = item * onepercent;
                        AnalyserByPercent.Add(queryPrcent);
                    }

                    return new ResponseAnalyserModelJobTypes()
                    {
                        AllCount = AllCount,
                        AnalyserByPercent = AnalyserByPercent
                    };
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// This <c>AnalyticsAsyncWorkLocationId </c> for the analytics by WorkLocationId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("AnalyticsByWorkLocations")]
        public async Task<ActionResult<ResponseAnalyserModelJobTypes>> AnalyticsAsyncWorkLocationId(RequestModelAnalyserWorklocationId model)
        {
            try
            {
                if (model.Equals(null) && model.WorkLocationId.Count == 0)
                {
                    return BadRequest();
                }

                if (model.WorkLocationId.Count == 1)
                {
                    var allCount = await _context.Journal.Where(x => x.WorkLocationId != null && x.DeletedDate.HasValue == model.IsTrash).CountAsync();
                    foreach (var item in model.WorkLocationId)
                    {

                        var query = _context.Journal.Where(x => x.WorkLocationId == item);
                        query = query.Where(x => x.DeletedDate.HasValue == model.IsTrash);

                        if (model.Archived.Equals(false))
                        {
                            query = query.Where(x => x.ArchivedDate >= DateTime.UtcNow);
                        }
                        if (model.IsTrash.Equals(false))
                        {
                            query = query.Where(x => x.DeletedDate == null);
                        }

                        double count = await query.CountAsync();
                        double percentformula = percent / allCount;

                        return Ok(percentformula * count);
                    }
                }
                if (model.WorkLocationId.Count > 1)
                {
                    foreach (var item in model.WorkLocationId)
                    {
                        var query = _context.Journal.Where(x => x.WorkLocationId == item);

                        if (model.Archived.Equals(false))
                        {
                            query = query.Where(x => x.ArchivedDate >= DateTime.UtcNow);
                        }
                        if (model.IsTrash.Equals(false))
                        {
                            query = query.Where(x => x.DeletedDate == null);
                        }
                        var count = query.CountAsync();

                        if (count != null && count.Result != 0)
                            AllCount.Add(await count);
                        else continue;

                    }
                    var sum = AllCount.Sum();
                    var onepercent = percent / sum;
                    foreach (var item in AllCount)
                    {
                        var queryPrcent = item * onepercent;
                        AnalyserByPercent.Add(queryPrcent);
                    }

                    return new ResponseAnalyserModelJobTypes()
                    {
                        AllCount = AllCount,
                        AnalyserByPercent = AnalyserByPercent
                    };
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// This <c>AnalyticsAsyncDepartmentId </c> for the analytics by DepartmentId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("AnalyticsByDepartments")]
        public async Task<ActionResult<ResponseAnalyserModelJobTypes>> AnalyticsAsyncDepartmentId(RequestModelAnalyserDepartmentId model)
        {
            try
            {

                if (model.Equals(null) && model.DepartmentId.Count == 0)
                {
                    return BadRequest();
                }

                if (model.DepartmentId.Count == 1)
                {
                    var allCount = await _context.Journal.Where(x => x.DepartmentId != null && x.DeletedDate.HasValue == model.IsTrash).CountAsync();
                    foreach (var item in model.DepartmentId)
                    {
                        var query = _context.Journal.Where(x => x.DepartmentId == item);
                        query = query.Where(x => x.DeletedDate.HasValue == model.IsTrash);

                        if (model.Archived.Equals(false))
                        {
                            query = query.Where(x => x.ArchivedDate >= DateTime.UtcNow);
                        }
                        if (model.IsTrash.Equals(false))
                        {
                            query = query.Where(x => x.DeletedDate == null);
                        }

                        double count = await query.CountAsync();
                        double percentformula = percent / allCount;

                        return Ok(percentformula * count);
                    }
                }

                if (model.DepartmentId.Count > 1)
                {
                    foreach (var item in model.DepartmentId)
                    {
                        var query = _context.Journal.Where(x => x.DepartmentId == item);

                        if (model.Archived.Equals(false))
                        {
                            query = query.Where(x => x.ArchivedDate >= DateTime.UtcNow);
                        }
                        if (model.IsTrash.Equals(false))
                        {
                            query = query.Where(x => x.DeletedDate == null);
                        }
                        var count = query.CountAsync();

                        if (count != null && count.Result != 0)
                            AllCount.Add(await count);
                        else continue;
                    }

                    var sum = AllCount.Sum();
                    var onepercent = percent / sum;
                    foreach (var item in AllCount)
                    {
                        var queryPrcent = item * onepercent;
                        AnalyserByPercent.Add(queryPrcent);
                    }
                    return new ResponseAnalyserModelJobTypes()
                    {
                        AllCount = AllCount,
                        AnalyserByPercent = AnalyserByPercent
                    };
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// This <c>AnalyticsAsyncJobTypeId </c> for the analytics by JobTypeId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("AnalyticsByJobTypes")]
        public async Task<ActionResult<ResponseAnalyserModelJobTypes>> AnalyticsAsyncJobTypeId(RequestModelAnalyzerJobTypes model)
        {
            try
            {
                if (model.Equals(null) && model.JobTypeId.Count == 0)
                {
                    return BadRequest();
                }

                if (model.JobTypeId.Count == 1)
                {
                    var allCount = await _context.Journal.Where(x => x.JobTypeId != null && x.DeletedDate.HasValue == model.IsTrash).CountAsync();
                    foreach (var item in model.JobTypeId)
                    {
                        var query = _context.Journal.Where(x => x.JobTypeId == item);
                        query = query.Where(x => x.DeletedDate.HasValue == model.IsTrash);

                        if (model.Archived.Equals(false))
                        {
                            query = query.Where(x => x.ArchivedDate >= DateTime.UtcNow);
                        }
                        if (model.IsTrash.Equals(false))
                        {
                            query = query.Where(x => x.DeletedDate == null);
                        }

                        double count = await query.CountAsync();
                        double percentformula = percent / allCount;

                        return Ok(percentformula * count);
                    }
                }

                if (model.JobTypeId.Count > 1)
                {
                    foreach (var item in model.JobTypeId)
                    {
                        var query = _context.Journal.Where(x => x.JobTypeId == item);

                        if (model.Archived.Equals(false))
                        {
                            query = query.Where(x => x.ArchivedDate >= DateTime.UtcNow);
                        }
                        if (model.IsTrash.Equals(false))
                        {
                            query = query.Where(x => x.DeletedDate == null);
                        }
                        var count = query.CountAsync();

                        if (count != null && count.Result != 0)
                            AllCount.Add(await count);
                        else continue;
                    }
                    var sum = AllCount.Sum();
                    var onepercent = percent / sum;
                    foreach (var item in AllCount)
                    {
                        var queryPrcent = item * onepercent;
                        AnalyserByPercent.Add(queryPrcent);
                    }

                    return new ResponseAnalyserModelJobTypes()
                    {
                        AllCount = AllCount,
                        AnalyserByPercent = AnalyserByPercent
                    };
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}