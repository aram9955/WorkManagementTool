using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkManagementTool.Data;
using WorkManagementTool.Models;
using WorkManagementTool.Models.RequestAnalyserModels;
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
        /// This <c>AnalyticsAsyncByFilter </c> for the analytics by Fillter
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("AnalyticsByFilter")]
        public async Task<ActionResult<ResponseAnalyserModel>> AnalyticsAsyncFillter(AnalyserFillters model)
        {
            try
            {
                if (model.DepartmentId != null && model.DepartmentId.Count != 0 || model.WorkLocationId != null && model.WorkLocationId.Count != 0
                     || model.JobTypeId != null && model.JobTypeId.Count != 0 || model.UserId != null && model.UserId.Count != 0)
                {
                    var query = _context.Journal.Where(x => x.Id != 0);
                    if (model.DepartmentId != null && model.DepartmentId.Count != 0)
                    {
                        foreach (var item in model.DepartmentId)
                        {
                            if (model.DepartmentId.Count > 1)
                            {
                                query = _context.Journal.Where(x => x.DepartmentId == item);
                            }
                            else
                            {
                                query = query.Where(x => x.DepartmentId == item);
                            }
                            var departmentCount = query.CountAsync();
                            if (departmentCount != null && departmentCount.Result != 0)
                            {
                                AllCount.Add(await departmentCount);
                            }
                        }
                    }
                    if (model.WorkLocationId != null && model.WorkLocationId.Count != 0)
                    {
                        foreach (var item in model.WorkLocationId)
                        {
                            if (model.WorkLocationId.Count > 1)
                            {
                                query = _context.Journal.Where(x => x.WorkLocationId == item);
                            }
                            else
                            {
                                query = query.Where(x => x.WorkLocationId == item);
                            }
                            var workLocationCount = query.CountAsync();
                            if (workLocationCount != null && workLocationCount.Result != 0)
                            {
                                AllCount.Add(await workLocationCount);
                            }
                        }
                    }
                    if (model.JobTypeId != null && model.JobTypeId.Count != 0)
                    {
                        foreach (var item in model.JobTypeId)
                        {
                            if (model.JobTypeId.Count > 1)
                            {
                                query = _context.Journal.Where(x => x.JobTypeId == item);
                            }
                            else
                            {
                                query = query.Where(x => x.JobTypeId == item);
                            }
                           
                            var jobTypeCount = query.CountAsync();
                            if (jobTypeCount != null && jobTypeCount.Result != 0)
                            {
                                AllCount.Add(await jobTypeCount);
                            }
                        }
                    }
                    if (model.UserId != null && model.UserId.Count != 0)
                    {
                        foreach (var item in model.UserId)
                        {
                            if (model.UserId.Count > 1)
                            {
                                query = _context.Journal.Where(x => x.UserId == item);
                            }
                            else
                            {
                                query = query.Where(x => x.UserId == item);
                            }
                            
                            var userCount = query.CountAsync();
                            if (userCount != null && userCount.Result != 0)
                            {
                                AllCount.Add(await userCount);
                            }
                        }
                    }
                    if (model.Archived.Equals(false))
                    {
                        query = query.Where(x => x.ArchivedDate >= DateTime.UtcNow);
                    }
                    if (model.IsTrash.Equals(false))
                    {
                        query = query.Where(x => x.DeletedDate == null);
                    }
                    if (model.StartDate.HasValue)
                    {
                        query = query.Where(x => model.StartDate <= x.CreateDate);
                    }
                    if (model.EndDate.HasValue)
                    {
                        query = query.Where(x => model.EndDate >= x.CreateDate);
                    }
                }

                double sum = AllCount.Sum();
                double onepercent = percent / sum;

                foreach (var item in AllCount)
                {
                    double queryPrcent = item * onepercent;
                    AnalyserByPercent.Add(queryPrcent);
                }


                return new ResponseAnalyserModel()
                {
                    AllCount = AllCount,
                    AnalyserByPercent = AnalyserByPercent
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
     
        }
        
            /// <summary>
            /// This <c>AnalyticsAsyncUserId </c> for the analytics by UserId
            /// </summary>
            /// <param name="model"></param>
            /// <returns></returns>
         [HttpGet("AnalyticsByUsers")]
        public async Task<ActionResult<ResponseAnalyserModel>> AnalyticsAsyncUserId(RequestModelAnalyserUserId model)
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
                        if (model.StartDate.HasValue)
                        {
                            query = query.Where(x => model.StartDate <= x.CreateDate);
                        }
                        if (model.EndDate.HasValue)
                        {
                            query = query.Where(x => model.EndDate >= x.CreateDate);
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
                        if (model.StartDate.HasValue)
                        {
                            query = query.Where(x => model.StartDate <= x.CreateDate);
                        }
                        if (model.EndDate.HasValue)
                        {
                            query = query.Where(x => model.EndDate >= x.CreateDate);
                        }

                        var count = query.CountAsync();

                        if (count != null && count.Result != 0)
                            AllCount.Add(await count);
                        else continue;


                    }
                    double sum = AllCount.Sum();
                    double onepercent = percent / sum;
                    foreach (var item in AllCount)
                    {
                        double queryPrcent = item * onepercent;
                        AnalyserByPercent.Add(queryPrcent);
                    }

                    return new ResponseAnalyserModel()
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
        public async Task<ActionResult<ResponseAnalyserModel>> AnalyticsAsyncWorkLocationId(RequestModelAnalyserWorklocationId model)
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
                        if (model.StartDate.HasValue)
                        {
                            query = query.Where(x => model.StartDate <= x.CreateDate);
                        }
                        if (model.EndDate.HasValue)
                        {
                            query = query.Where(x => model.EndDate >= x.CreateDate);
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
                        if (model.StartDate.HasValue)
                        {
                            query = query.Where(x => model.StartDate <= x.CreateDate);
                        }
                        if (model.EndDate.HasValue)
                        {
                            query = query.Where(x => model.EndDate >= x.CreateDate);
                        }


                        var count = query.CountAsync();

                        if (count != null && count.Result != 0)
                            AllCount.Add(await count);
                        else continue;

                    }
                    double sum = AllCount.Sum();
                    double onepercent = percent / sum;
                    foreach (var item in AllCount)
                    {
                        double queryPrcent = item * onepercent;
                        AnalyserByPercent.Add(queryPrcent);
                    }

                    return new ResponseAnalyserModel()
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
        public async Task<ActionResult<ResponseAnalyserModel>> AnalyticsAsyncDepartmentId(RequestModelAnalyserDepartmentId model)
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
                        if (model.StartDate.HasValue)
                        {
                            query = query.Where(x => model.StartDate <= x.CreateDate);
                        }
                        if (model.EndDate.HasValue)
                        {
                            query = query.Where(x => model.EndDate >= x.CreateDate);
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
                        if (model.StartDate.HasValue)
                        {
                            query = query.Where(x => model.StartDate <= x.CreateDate);
                        }
                        if (model.EndDate.HasValue)
                        {
                            query = query.Where(x => model.EndDate >= x.CreateDate);
                        }

                        var count = query.CountAsync();

                        if (count != null && count.Result != 0)
                            AllCount.Add(await count);
                        else continue;
                    }

                    double sum = AllCount.Sum();
                    double onepercent = percent / sum;
                    foreach (var item in AllCount)
                    {
                        double queryPrcent = item * onepercent;
                        AnalyserByPercent.Add(queryPrcent);
                    }
                    return new ResponseAnalyserModel()
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
        public async Task<ActionResult<ResponseAnalyserModel>> AnalyticsAsyncJobTypeId(RequestModelAnalyzerJobTypes model)
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
                        if (model.StartDate.HasValue)
                        {
                            query = query.Where(x => model.StartDate <= x.CreateDate);
                        }
                        if (model.EndDate.HasValue)
                        {
                            query = query.Where(x => model.EndDate >= x.CreateDate);
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
                        if (model.StartDate.HasValue)
                        {
                            query = query.Where(x => model.StartDate <= x.CreateDate);
                        }
                        if (model.EndDate.HasValue)
                        {
                            query = query.Where(x => model.EndDate >= x.CreateDate);
                        }

                        var count = query.CountAsync();

                        if (count != null && count.Result != 0)
                            AllCount.Add(await count);
                        else continue;
                    }
                    double sum = AllCount.Sum();
                    double onepercent = percent / sum;
                    foreach (var item in AllCount)
                    {
                        double queryPrcent = item * onepercent;
                        AnalyserByPercent.Add(queryPrcent);
                    }

                    return new ResponseAnalyserModel()
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