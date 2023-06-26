using DataTech.System.Versioning.Data;
using DataTech.System.Versioning.Models.Common;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using DataTech.System.Versioning.Models.Domain;
using DataTech.System.Versioning.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using DataTech.System.Versioning.Models.Dto.System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Web.Http.Results;

namespace DataTech.System.Versioning.Repositories
{
    public class SystemRepository : ISystemRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<SystemRepository> _logger;
        public SystemRepository(DataContext context, ILogger<SystemRepository> logger)
        {
            _context = context;
            _logger = logger;
        }



        public async Task<OperationResult<AppSystem>> Insert(Query<AppSystem> query)
        {
            var result = new OperationResult<AppSystem>();
            try
            {
                if (query == null || query.Parameter == null)
                {
                    result.PrepareMissingParameterResult("Parameter");
                    return result;
                }

                if (query.Parameter.Name.IsEmpty())
                {
                    result.PrepareMissingParameterResult("Name");
                    return result;
                }

                var check = await _context.AppModules.FirstOrDefaultAsync(x => x.Name == query.Parameter.Name);

                if (check != null)
                {
                    result.PrepareExistedPropertyResult("Name");
                    return result;
                }

                query.Parameter.ReleaseIndex = query.Parameter.ReleaseIndex < 1 ? 1 : query.Parameter.ReleaseIndex;

                await _context.AppSystems.AddAsync(query.Parameter);

                var log = new AppSystemLog
                {
                    AppSystemId = query.Parameter.Id,
                    Description = "",
                    ReleaseIndex = query.Parameter.ReleaseIndex
                };

                await _context.AppSystemLogs.AddAsync(log);

                await _context.SaveChangesAsync();
                result.Result = true;
                result.Response = query.Parameter;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding {@query}", query);
                result.PrepareExceptionResult(ex);
            }
            return result;
        }

        public async Task<OperationResult<AppSystem>> Delete(Query<AppSystem> query)
        {
            var result = new OperationResult<AppSystem>();
            try
            {
                if (query == null || query.Parameter == null)
                {
                    result.PrepareMissingParameterResult("Parameter");
                    return result;
                }

                if (query.Parameter.Id == Guid.Empty)
                {
                    result.PrepareMissingParameterResult("Id");
                    return result;
                }

                var dbAppResult = await _context.AppSystems.FirstOrDefaultAsync(x => x.Id == query.Parameter.Id);

                if (dbAppResult == null)
                {
                    result.PrepareNotFoundResult();
                    return result;
                }
                dbAppResult.Deleted = true;
                _context.AppSystems.Update(dbAppResult);
                await _context.SaveChangesAsync();
                result.Result = true;
                result.Response = dbAppResult;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting {@query}", query);
                result.PrepareExceptionResult(ex);
            }
            return result;
        }

        public async Task<OperationResult<AppSystem>> GetById(Query<AppSystem> query)
        {
            var result = new OperationResult<AppSystem>();
            try
            {
                if (query == null || query.Parameter == null)
                {
                    result.PrepareMissingParameterResult("Parameter");
                    return result;
                }

                if (query.Parameter.Id == Guid.Empty)
                {
                    result.PrepareMissingParameterResult("Id");
                    return result;
                }

                var respone = await _context.AppSystems
                       .Include(x => x.Modules)
                        .FirstOrDefaultAsync(x => x.Id == query.Parameter.Id);

                if (respone != null)
                {
                    result.Result = true;
                    result.Response = respone;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting {@query}", query);
                result.PrepareExceptionResult(ex);
            }
            return result;
        }



        public async Task<PaggingOperationResult<AppSystem>> Search(Query<AppSystem> query)
        {
            var result = new PaggingOperationResult<AppSystem>();
            try
            {
                if (query == null || query.Parameter == null)
                {
                    result.PrepareMissingParameterResult("Parameter");
                    return result;
                }

                var appSystems = _context.AppSystems
                                    .Include(x => x.Modules) 
                                    .Where(a => !a.Deleted)
                                    .Select(a => a);


                if (!string.IsNullOrEmpty(query.Parameter.Name))
                {

                    appSystems = appSystems.Where(r => EF.Functions.Like((
                        (r.Name ?? string.Empty)
                        ), $"%{query.Parameter.Name.Trim().Replace(" ", "%")}%"));
                }

                result.TotalCount = await appSystems.CountAsync();
                result.Response = await appSystems.OrderBy(query).Page(query.PageSize, query.PageIndex).ToListAsync();
                result.Result = true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching {@query}", query);
                result.PrepareExceptionResult(ex);
            }
            return result;
        }

        public async Task<OperationResult<AppSystem>> Update(Query<AppSystem> query)
        {
            var result = new OperationResult<AppSystem>();
            try
            {
                if (query == null || query.Parameter == null)
                {
                    result.PrepareMissingParameterResult("Parameter");
                    return result;
                }

                if (query.Parameter.Id == Guid.Empty)
                {
                    result.PrepareMissingParameterResult("Id");
                    return result;
                }
                if (query.Parameter.Name.IsEmpty())
                {
                    result.PrepareMissingParameterResult("Name");
                    return result;
                }

                var dbAppResult = await _context.AppSystems.FirstOrDefaultAsync(x => x.Id == query.Parameter.Id);

                if (dbAppResult == null)
                {
                    result.PrepareNotFoundResult();
                    return result;
                }


                var check = await _context.AppSystems.FirstOrDefaultAsync(x => x.Name == query.Parameter.Name && x.Id != query.Parameter.Id);

                if (check != null)
                {
                    result.PrepareExistedPropertyResult("Name");
                    return result;
                }

                dbAppResult.Name = query.Parameter.Name;
                _context.AppSystems.Update(dbAppResult);
                await _context.SaveChangesAsync();
                result.Result = true;
                result.Response = dbAppResult;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating {@query}", query);
                result.PrepareExceptionResult(ex);
            }
            return result;
        }

        public async Task<OperationResult<AppSystem>> GetWihLogs(Query<AppSystem> query)
        {
            var result = new OperationResult<AppSystem>();
            try
            {
                if (query == null || query.Parameter == null)
                {
                    result.PrepareMissingParameterResult("Parameter");
                    return result;
                }

                if (query.Parameter.Id == Guid.Empty)
                {
                    result.PrepareMissingParameterResult("Id");
                    return result;
                }

                var respone = await _context.AppSystems
                       .Include(x => x.Modules)
                        .Include(x => x.Logs)
                        .FirstOrDefaultAsync(x => x.Id == query.Parameter.Id);

                if (respone != null)
                {
                    result.Result = true;
                    result.Response = respone;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting with logs {@query}", query);
                result.PrepareExceptionResult(ex);
            }
            return result;
        }

        public async Task<OperationResult<AppSystemLog>> AddNewRelease(Query<AppSystemLog> query)
        {
            var result = new OperationResult<AppSystemLog>();
            try
            {
                if (query == null || query.Parameter == null)
                {
                    result.PrepareMissingParameterResult("Parameter");
                    return result;
                }

                if (query.Parameter.AppSystemId == Guid.Empty)
                {
                    result.PrepareMissingParameterResult("AppSystemId");
                    return result;
                }

                var getApp = await _context.AppSystems.FirstOrDefaultAsync(x => x.Id == query.Parameter.AppSystemId);

                if (getApp == null)
                {
                    result.PrepareNotFoundResult();
                    return result;
                }

                getApp.ReleaseIndex++;

                _context.AppSystems.Update(getApp);

                var log = new AppSystemLog
                {
                    AppSystemId = query.Parameter.AppSystemId,
                    Description = query.Parameter.Description,
                    ReleaseIndex = getApp.ReleaseIndex
                };

                await _context.AppSystemLogs.AddAsync(log);

                await _context.SaveChangesAsync();

                await UpdateAppIndexer(getApp);

                result.Result = true;
                result.Response = log;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding log {@query}", query);
                result.PrepareExceptionResult(ex);
            }
            return result;
        }

        private async Task UpdateAppIndexer(AppSystem appSystem)
        {
            try
            {
                var allIndexers = await _context.AppIndexers.Where(x => x.AppSystemId == appSystem.Id).ToListAsync();

                if (allIndexers.Count > 0)
                {
                    foreach (var indexer in allIndexers)
                    {
                        indexer.ReleaseIndex = appSystem.ReleaseIndex;
                        indexer.VersionIndex = 1;
                        indexer.EnhancementIndex = 0;
                        indexer.FixIndex = 0;
                    }

                    _context.UpdateRange(allIndexers);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating indexer {@appSystem}", appSystem);
            }
        }

        public async Task<OperationResult<AppSystemLog>> EditRelease(Query<AppSystemLog> query)
        {
            var result = new OperationResult<AppSystemLog>();
            try
            {
                if (query == null || query.Parameter == null)
                {
                    result.PrepareMissingParameterResult("Parameter");
                    return result;
                }

                if (query.Parameter.Id == Guid.Empty)
                {
                    result.PrepareMissingParameterResult("Id");
                    return result;
                }
                if (query.Parameter.Description.IsEmpty())
                {
                    result.PrepareMissingParameterResult("Description");
                    return result;
                }

                var dbAppResult = await _context.AppSystemLogs.FirstOrDefaultAsync(x => x.Id == query.Parameter.Id);

                if (dbAppResult == null)
                {
                    result.PrepareNotFoundResult();
                    return result;
                }

                dbAppResult.Description = query.Parameter.Description;
                _context.AppSystemLogs.Update(dbAppResult);
                await _context.SaveChangesAsync();
                result.Result = true;
                result.Response = dbAppResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing release log {@query}", query);
                result.PrepareExceptionResult(ex);
            }
            return result;
        }

        public async Task<OperationResult<AppSystem>> GetByName(Query<GetSystemVersionRequest> query)
        {
            var result = new OperationResult<AppSystem>();
            try
            {
                if (query == null || query.Parameter == null)
                {
                    result.PrepareMissingParameterResult("Parameter");
                    return result;
                }

                if (query.Parameter.Name.IsEmpty())
                {
                    result.PrepareMissingParameterResult("Name");
                    return result;
                }

                var respone = await _context.AppSystems
                     .Include(x => x.Logs)
                     .Include(x => x.Modules)
                              .ThenInclude(y => y.Logs)
                     .FirstOrDefaultAsync(x => x.Name == query.Parameter.Name &&
                            (query.Parameter.ReleaseIndex < 1 || x.ReleaseIndex == query.Parameter.ReleaseIndex));


                if (respone != null)
                {
                    result.Result = true;
                    result.Response = respone;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting by Name {@query}", query);
                result.PrepareExceptionResult(ex);
            }
            return result;
        }
    }
}
