using DataTech.System.Versioning.Extensions;
using DataTech.System.Versioning.Models.Common;
using DataTech.System.Versioning.Models.Domain;
using DataTech.System.Versioning.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DataTech.System.Versioning.Services
{
    public class SystemService : ISystemService
    {
        private readonly ISystemRepository _repository;
        private readonly ILogger<SystemService> _logger; 

        public SystemService(ISystemRepository repository, ILogger<SystemService> logger)
        {
            _repository = repository;
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

                return await _repository.Insert(query);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding {@query}", query);
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

                return await _repository.AddNewRelease(query);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding log {@query}", query);
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

                return await _repository.Delete(query);

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

                return await _repository.GetById(query);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting {@query}", query);
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

                return await _repository.GetWihLogs(query);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting with logs {@query}", query);
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
                return await _repository.Search(query);

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

                return await _repository.EditRelease(query);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing release {@query}", query);
                result.PrepareExceptionResult(ex);
            }
            return result;
        }
    }
}
