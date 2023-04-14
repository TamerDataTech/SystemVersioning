using DataTech.System.Versioning.Models.Common;
using DataTech.System.Versioning.Models.Domain;
using DataTech.System.Versioning.Models.Request;
using System;
using System.Threading.Tasks;

namespace DataTech.System.Versioning.Services
{
    public interface IUserService
    {
        Task<OperationResult<AppUser>> Login(LoginRequest request); 
        Task<OperationResult<AppUser>> GetUserById(Guid id);
    }
}
