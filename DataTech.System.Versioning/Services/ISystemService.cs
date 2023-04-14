using DataTech.System.Versioning.Models.Common;
using DataTech.System.Versioning.Models.Domain;
using System.Threading.Tasks;

namespace DataTech.System.Versioning.Services
{
    public interface ISystemService
    {
        Task<OperationResult<AppSystem>> Insert(Query<AppSystem> query);
        Task<OperationResult<AppSystem>> Delete(Query<AppSystem> query);
        Task<OperationResult<AppSystem>> GetById(Query<AppSystem> query);
        Task<PaggingOperationResult<AppSystem>> Search(Query<AppSystem> query);
        Task<OperationResult<AppSystem>> Update(Query<AppSystem> query);

        Task<OperationResult<AppSystem>> GetWihLogs(Query<AppSystem> query); 
        Task<OperationResult<AppSystemLog>> AddNewRelease(Query<AppSystemLog> query);
        Task<OperationResult<AppSystemLog>> EditRelease(Query<AppSystemLog> query);



    }
}
