using DataTech.System.Versioning.Models.Common;
using DataTech.System.Versioning.Models.Domain;
using System.Threading.Tasks;
using System;
using DataTech.System.Versioning.Repositories;
using Microsoft.Extensions.Logging;
using DataTech.System.Versioning.Extensions;

namespace DataTech.System.Versioning.Services
{
    public class ModuleService : IModuleService
    {
        private readonly IModuleRepository _repository;
        private readonly ILogger<ModuleService> _logger;
        public ModuleService(IModuleRepository repository, ILogger<ModuleService> logger)
        {
            _repository = repository;
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

                return await _repository.Insert(query);

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

                return await _repository.Delete(query);

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

                return await _repository.GetById(query);

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
                return await _repository.Search(query);

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

                return await _repository.Update(query);

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

                return await _repository.GetWihLogs(query);

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

                return await _repository.AddNewVersion(query);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding version {@query}", query);
                result.PrepareExceptionResult(ex);
            }
            return result;
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

                return await _repository.EditVersion(query);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing version {@query}", query);
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

                return await _repository.AddNewEnhancement(query);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding enhancement {@query}", query);
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

                return await _repository.AddNewFix(query);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding fix {@query}", query);
                result.PrepareExceptionResult(ex);
            }
            return result;
        }

        public async Task<OperationResult<AppModuleLog>> EditEnhancement(Query<AppModuleLog> query)
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

                return await _repository.EditEnhancement(query);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing enhancement {@query}", query);
                result.PrepareExceptionResult(ex);
            }
            return result;
        }

        public async Task<OperationResult<AppModuleLog>> EditFix(Query<AppModuleLog> query)
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

                return await _repository.EditFix(query);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing fix {@query}", query);
                result.PrepareExceptionResult(ex);
            }
            return result;
        }
         
    }
}
