using DataTech.System.Versioning.Extensions;
using DataTech.System.Versioning.Models.Common;
using DataTech.System.Versioning.Models.Domain;
using DataTech.System.Versioning.Models.Request;
using DataTech.System.Versioning.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DataTech.System.Versioning.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }


        public async Task<OperationResult<AppUser>> Login(LoginRequest request)
        {
            var result = new OperationResult<AppUser>();

            try
            {
                if (request == null)
                {
                    result.PrepareMissingParameterResult("Parameter");
                    return result;
                }

                if (request.Username.IsEmpty())
                {
                    result.PrepareMissingParameterResult("Username");
                    return result;
                }

                if (request.Password.IsEmpty())
                {
                    result.PrepareMissingParameterResult("Password");
                    return result;
                }

                return await _userRepository.Login(request);
            }
            catch (Exception ex)
            {
                result.PrepareExceptionResult(ex);
                _logger.LogError(ex, "DoLogin Exception {@request}", request);
            }
            
            return result;
        }

        public async Task<OperationResult<AppUser>> GetUserById(Guid id)
        {
            var result = new OperationResult<AppUser>();

            try
            {
                if (id == Guid.Empty)
                {
                    result.PrepareMissingParameterResult("id");
                    return result;
                } 

                return await _userRepository.GetUserById(id);
            }
            catch (Exception ex)
            {
                result.PrepareExceptionResult(ex);
                _logger.LogError(ex, "GetUserById Exception {@id}", id);
            }

            return result;
        }
    }
}
