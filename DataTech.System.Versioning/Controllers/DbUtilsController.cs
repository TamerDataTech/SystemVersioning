using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; 
using DataTech.System.Versioning.Service.Db; 
using System.Threading.Tasks;
using DatabaseUtils.Model.Database;
using DataTech.System.Versioning.Models.Database.Request;

namespace DataTech.System.Versioning.Controllers
{
    public class DbUtilsController : AuthController
    {
        private readonly ILogger<DbUtilsController> _logger;
        private readonly DatabaseService _databaseService;
        public DbUtilsController(ILogger<DbUtilsController> logger, DatabaseService databaseService)
        {
            _logger = logger;
            _databaseService = databaseService;
        }

        public IActionResult Backup()
        {
            return View();
        }


        public IActionResult Load()
        {
            return View();
        }

        public async Task<IActionResult> GetDatabases(DbConnectionString conn)
        {
            return Ok(await _databaseService.GetDataBases(new DbQuery<Database> { Conn = conn }));
        }

        public async Task<IActionResult> GetTables(DbConnectionString conn)
        {
            return Ok(await _databaseService.GetDataBaseTables(new DbQuery<Database> { Conn = conn }));
        }
         
        public async Task<IActionResult> CreateTableBackup(DbQuery<BackupTablesRequest> query)
        {
            return Ok(await _databaseService.CreateTableBackup(query));
        }
    }
}
