using DataTech.System.Versioning.Data;
using DataTech.System.Versioning.Extensions;
using DataTech.System.Versioning.Models.Common;
using DataTech.System.Versioning.Models.Domain;
using DataTech.System.Versioning.Models.Request;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks; 
using Microsoft.EntityFrameworkCore;
using System;

namespace DataTech.System.Versioning.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context; 
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(DataContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
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

                var getUserResult = await _context.AppUsers.FirstOrDefaultAsync(x => x.Id == id);

                if (getUserResult != null)
                { 
                    result.Response = getUserResult;
                    result.Result = true;
                }
                else
                {
                    result.ErrorMessage = "User is not existed";
                }
            }
            catch (Exception ex)
            {
                result.PrepareExceptionResult(ex);
                _logger.LogError(ex, "GetUserById Exception {@id}", id);
            }

            return result;
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


                var getUserResult = await _context.AppUsers.FirstOrDefaultAsync(x => x.Username == request.Username || x.Email == request.Username);

                if (getUserResult != null)
                {

                    string encryptedPassword = Crypto.Encrypt(request.Password);

                    if (getUserResult.Password == encryptedPassword)
                    {
                        result.Response = getUserResult;
                        result.Result = true;
                    }
                    else
                    {
                        result.ErrorMessage = "Password is wrong";
                    }
                }
                else
                {
                    result.ErrorMessage = "Username is not existed";
                }
            }
            catch (Exception ex)
            {
                result.PrepareExceptionResult(ex);
                _logger.LogError(ex, "DoLogin Exception {@request}", request);
            }

            return result;

        }
    }
}
