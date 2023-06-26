using DataTech.System.Versioning.Data;
using DataTech.System.Versioning.Models.Common;
using DataTech.System.Versioning.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using System;
using DataTech.System.Versioning.Extensions;

namespace DataTech.System.Versioning.Repositories
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<ModuleRepository> _logger;
        public ModuleRepository(DataContext context, ILogger<ModuleRepository> logger)
        {
            _context = context;
            _logger = logger;
        }



        public async Task<OperationResult<AppModule>> Insert(Query<AppModule> query)
        {
            var result = new OperationResult<AppModule>();
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

                if (query.Parameter.AppSystemId == Guid.Empty)
                {
                    result.PrepareMissingParameterResult("AppSystemId");
                    return result;
                }

                var check = await _context.AppModules.FirstOrDefaultAsync(x => x.Name == query.Parameter.Name);

                if (check != null)
                {
                    result.PrepareExistedPropertyResult("Name");
                    return result;
                }

                var appSystem = await _context.AppSystems.FirstOrDefaultAsync(x => x.Id == query.Parameter.AppSystemId);

                if (check != null)
                {
                    result.PrepareNotFoundResult("System");
                    return result;
                }

                query.Parameter.VersionIndex = appSystem.ReleaseIndex; 
                query.Parameter.VersionIndex = 1;

                await _context.AppModules.AddAsync(query.Parameter);  
                await _context.SaveChangesAsync();

                var log = new AppModuleLog
                { 
                    AppModuleId = query.Parameter.Id,
                    Description = "",
                    VersionIndex = query.Parameter.VersionIndex,
                    EnhancementIndex = query.Parameter.EnhancementIndex,
                    FixIndex = query.Parameter.FixIndex
                }; 
                await _context.AppModuleLogs.AddAsync(log); 

                // Add Indexer
                var indexer = new AppIndexer
                {
                    AppSystemId = appSystem.Id,
                    AppModuleId =   query.Parameter.Id, 
                    ReleaseIndex = appSystem.ReleaseIndex,
                    VersionIndex = query.Parameter.VersionIndex,
                    EnhancementIndex = 0,
                    FixIndex = 0
                };
                await _context.AppIndexers.AddAsync(indexer);

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

        public async Task<OperationResult<AppModule>> Delete(Query<AppModule> query)
        {
            var result = new OperationResult<AppModule>();
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

                var dbAppResult = await _context.AppModules.FirstOrDefaultAsync(x => x.Id == query.Parameter.Id);

                if (dbAppResult == null)
                {
                    result.PrepareNotFoundResult();
                    return result;
                }

                dbAppResult.Deleted = true;
                _context.AppModules.Update(dbAppResult);
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

        public async Task<OperationResult<AppModule>> GetById(Query<AppModule> query)
        {
            var result = new OperationResult<AppModule>();
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

                var respone = await _context.AppModules
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



        public async Task<PaggingOperationResult<AppModule>> Search(Query<AppModule> query)
        {
            var result = new PaggingOperationResult<AppModule>();
            try
            {
                if (query == null || query.Parameter == null)
                {
                    result.PrepareMissingParameterResult("Parameter");
                    return result;
                }

                var AppModules = _context.AppModules
                                    .Where(a => !a.Deleted)
                                    .Select(a => a);


                if (!string.IsNullOrEmpty(query.Parameter.Name))
                {

                    AppModules = AppModules.Where(r => EF.Functions.Like((
                        (r.Name ?? string.Empty)
                        ), $"%{query.Parameter.Name.Trim().Replace(" ", "%")}%"));
                }

                result.TotalCount = await AppModules.CountAsync();
                result.Response = await AppModules.OrderBy(query).Page(query.PageSize, query.PageIndex).ToListAsync();
                result.Result = true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching {@query}", query);
                result.PrepareExceptionResult(ex);
            }
            return result;
        }

        public async Task<OperationResult<AppModule>> Update(Query<AppModule> query)
        {
            var result = new OperationResult<AppModule>();
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
                    result.PrepareMissingParameterResult("ElementsAppId");
                    return result;
                }

                var dbAppResult = await _context.AppModules.FirstOrDefaultAsync(x => x.Id == query.Parameter.Id);

                if (dbAppResult == null)
                {
                    result.PrepareNotFoundResult();
                    return result;
                }


                var check = await _context.AppModules.FirstOrDefaultAsync(x => x.Name == query.Parameter.Name && x.Id != query.Parameter.Id);

                if (check != null)
                {
                    result.PrepareExistedPropertyResult("Name");
                    return result;
                }

                dbAppResult.Name = query.Parameter.Name;
                _context.AppModules.Update(dbAppResult);
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

        public async Task<OperationResult<AppModule>> GetWihLogs(Query<AppModule> query)
        {
            var result = new OperationResult<AppModule>();
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

                var respone = await _context.AppModules
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

        public async Task<OperationResult<AppModuleLog>> AddNewVersion(Query<AppModuleLog> query)
        {
            var result = new OperationResult<AppModuleLog>();
            try
            {
                if (query == null || query.Parameter == null)
                {
                    result.PrepareMissingParameterResult("Parameter");
                    return result;
                }

                if (query.Parameter.AppModuleId == Guid.Empty)
                {
                    result.PrepareMissingParameterResult("AppModuleId");
                    return result;
                } 


                var getApp = await _context.AppModules.FirstOrDefaultAsync(x => x.Id == query.Parameter.AppModuleId);

                if (getApp == null)
                {
                    result.PrepareNotFoundResult();
                    return result;
                } 
 
                var indexer = await GetCurrentModuleIndexer(query.Parameter.AppModuleId); 

                getApp.ReleaseIndex = indexer.ReleaseIndex;
                getApp.VersionIndex = indexer.VersionIndex + 1;
                getApp.EnhancementIndex = 0;
                getApp.FixIndex = 0;
                _context.AppModules.Update(getApp);

                var log = new AppModuleLog
                {
                    AppModuleId = query.Parameter.AppModuleId,
                    Description = query.Parameter.Description,
                    VersionIndex = getApp.VersionIndex,
                    EnhancementIndex = getApp.EnhancementIndex,
                    FixIndex = getApp.FixIndex
                };

                await _context.AppModuleLogs.AddAsync(log);





                await _context.SaveChangesAsync();

                result.Result = true;
                result.Response = log;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new verion {@query}", query);
                result.PrepareExceptionResult(ex);
            }
            return result;
        }


        private async Task<AppIndexer> GetCurrentModuleIndexer(Guid appModuleId)
        {
            return await _context.AppIndexers.FirstOrDefaultAsync(z=> z.AppModuleId == appModuleId);
        }

        public async Task<OperationResult<AppModuleLog>> EditVersion(Query<AppModuleLog> query)
        {
            var result = new OperationResult<AppModuleLog>();
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

                var dbAppResult = await _context.AppModuleLogs.FirstOrDefaultAsync(x => x.Id == query.Parameter.Id);

                if (dbAppResult == null)
                {
                    result.PrepareNotFoundResult();
                    return result;
                }

                dbAppResult.Description = query.Parameter.Description;
                _context.AppModuleLogs.Update(dbAppResult);
                await _context.SaveChangesAsync();
                result.Result = true;
                result.Response = dbAppResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing version log {@query}", query);
                result.PrepareExceptionResult(ex);
            }
            return result;
        }

        public async Task<OperationResult<AppModuleLog>> AddNewEnhancement(Query<AppModuleLog> query)
        {
            var result = new OperationResult<AppModuleLog>();
            try
            {
                if (query == null || query.Parameter == null)
                {
                    result.PrepareMissingParameterResult("Parameter");
                    return result;
                }

                if (query.Parameter.AppModuleId == Guid.Empty)
                {
                    result.PrepareMissingParameterResult("AppModuleId");
                    return result;
                }

                var getApp = await _context.AppModules.FirstOrDefaultAsync(x => x.Id == query.Parameter.AppModuleId);

                if (getApp == null)
                {
                    result.PrepareNotFoundResult();
                    return result;
                }

                var indexer = await GetCurrentModuleIndexer(query.Parameter.AppModuleId);

                getApp.ReleaseIndex = indexer.ReleaseIndex;
                getApp.VersionIndex = indexer.VersionIndex;
                getApp.EnhancementIndex = indexer.EnhancementIndex + 1;
                getApp.FixIndex = 0;

                _context.AppModules.Update(getApp);

                var log = new AppModuleLog
                {
                    AppModuleId = query.Parameter.AppModuleId,
                    Description = query.Parameter.Description,
                    ReleaseIndex = getApp.ReleaseIndex,
                    VersionIndex = getApp.VersionIndex,
                    EnhancementIndex = getApp.EnhancementIndex,
                    FixIndex = getApp.FixIndex
                };
                await _context.AppModuleLogs.AddAsync(log);

                indexer.EnhancementIndex++; 
                _context.Update(indexer);

                await _context.SaveChangesAsync();

                result.Result = true;
                result.Response = log;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new update {@query}", query);
                result.PrepareExceptionResult(ex);
            }
            return result;
        }

        public async Task<OperationResult<AppModuleLog>> AddNewFix(Query<AppModuleLog> query)
        {
            var result = new OperationResult<AppModuleLog>();
            try
            {
                if (query == null || query.Parameter == null)
                {
                    result.PrepareMissingParameterResult("Parameter");
                    return result;
                }

                if (query.Parameter.AppModuleId == Guid.Empty)
                {
                    result.PrepareMissingParameterResult("AppModuleId");
                    return result;
                }

                var getApp = await _context.AppModules.FirstOrDefaultAsync(x => x.Id == query.Parameter.AppModuleId);

                if (getApp == null)
                {
                    result.PrepareNotFoundResult();
                    return result;
                }

                var indexer = await GetCurrentModuleIndexer(query.Parameter.AppModuleId);

                getApp.ReleaseIndex = indexer.ReleaseIndex;
                getApp.VersionIndex = indexer.VersionIndex;
                getApp.EnhancementIndex = indexer.EnhancementIndex;
                getApp.FixIndex = indexer.FixIndex + 1;

                _context.AppModules.Update(getApp);

                var log = new AppModuleLog
                {
                    AppModuleId = query.Parameter.AppModuleId,
                    Description = query.Parameter.Description,
                    ReleaseIndex = getApp.ReleaseIndex,
                    VersionIndex = getApp.VersionIndex,
                    EnhancementIndex = getApp.EnhancementIndex,
                    FixIndex = getApp.FixIndex
                };
                await _context.AppModuleLogs.AddAsync(log);

                indexer.FixIndex++;
                _context.Update(indexer);

                await _context.SaveChangesAsync();

                result.Result = true;
                result.Response = log;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new update {@query}", query);
                result.PrepareExceptionResult(ex);
            }
            return result;
        }

        public async Task<OperationResult<AppModuleLog>> EditEnhancement(Query<AppModuleLog> query)
        {
            return await EditVersion(query);
        }

        public async Task<OperationResult<AppModuleLog>> EditFix(Query<AppModuleLog> query)
        {
            return await EditVersion(query);
        }
    }
}
