using DataTech.System.Versioning.Models.Common;
using DataTech.System.Versioning.Models.Domain;
using System.Threading.Tasks;

namespace DataTech.System.Versioning.Repositories
{
    public interface IModuleRepository
    {
        Task<OperationResult<AppModule>> Insert(Query<AppModule> query);
        Task<OperationResult<AppModule>> Delete(Query<AppModule> query);
        Task<OperationResult<AppModule>> GetById(Query<AppModule> query);
        Task<PaggingOperationResult<AppModule>> Search(Query<AppModule> query);
        Task<OperationResult<AppModule>> Update(Query<AppModule> query);

        Task<OperationResult<AppModule>> GetWihLogs(Query<AppModule> query);
        Task<OperationResult<AppModuleLog>> AddNewVersion(Query<AppModuleLog> query);
        Task<OperationResult<AppModuleLog>> EditVersion(Query<AppModuleLog> query);
        Task<OperationResult<AppModuleLog>> AddNewUpdate(Query<AppModuleLog> query);
        Task<OperationResult<AppModuleLog>> EditUpdate(Query<AppModuleLog> query);
    }
}
