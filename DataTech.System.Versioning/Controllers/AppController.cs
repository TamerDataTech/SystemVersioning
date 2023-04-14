using DataTech.System.Versioning.Models.DataTable;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using DataTech.System.Versioning.Models.Domain;
using Microsoft.AspNetCore.Builder;
using DataTech.System.Versioning.Services;
using DataTech.System.Versioning.Models.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DataTech.System.Versioning.Controllers
{
    public class AppController : AuthController
    {
        private readonly ILogger<AppController> _logger;
        private readonly ISystemService _systemService;
        private readonly IModuleService _moduleService; 

        public AppController(ILogger<AppController> logger, ISystemService systemService, IModuleService moduleService)
        {
            _logger = logger;
            _systemService = systemService;
            _moduleService = moduleService;
        }

        public IActionResult Systems()
        {
            return View();
        }

        public async Task<IActionResult> SearchSystems(AppSystem request)
        {
            try
            {
                DataTableOptions options;
                var query = GetSearchQuery<AppSystem>(out options);
                query.Parameter = request ?? new AppSystem();
                var result = await _systemService.Search(query);

                return DTResult(options, result, result.Response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SearchSystems ");
            }
            return DTResult<AppSystem>();
        }

        public async Task<IActionResult> SetSystem(AppSystem request)
        {
            return Ok(request.Id == Guid.Empty ?
                                await _systemService.Insert(new Query<AppSystem>(request)) :
                                await _systemService.Update(new Query<AppSystem>(request)));
        }



        public async Task<IActionResult> GetSystem(AppSystem request)
        {
            return Ok(await _systemService.GetById(new Query<AppSystem>(request)));
        }

        public async Task<IActionResult> DeleteSystem(AppSystem request)
        {
            return Ok(await _systemService.Delete(new Query<AppSystem>(request)));
        }

        public async Task<IActionResult> GetSystemLogs(AppSystem request)
        {
            return Ok(await _systemService.GetWihLogs(new Query<AppSystem>(request)));
        }

        public async Task<IActionResult> AddNewRelease(AppSystemLog request)
        {
            return Ok(await _systemService.AddNewRelease(new Query<AppSystemLog>(request)));
        }
        public async Task<IActionResult> EditRelease(AppSystemLog request)
        {
            return Ok(await _systemService.EditRelease(new Query<AppSystemLog>(request)));
        }

        public async Task<IActionResult> GetAllSystems(AppSystem request)
        {
            var result = await _systemService.Search(new Query<AppSystem>
            {
                PageSize = int.MaxValue,
                OrderBy = "Name", 
                OrderDir = OrderDir.ASC,
                Parameter = new AppSystem { }
            });

            return Ok(result);
        }


        public IActionResult Modules()
        {
            return View();
        }

        public async Task<IActionResult> SearchModules(AppModule request)
        {
            try
            {
                DataTableOptions options;
                var query = GetSearchQuery<AppModule>(out options);
                query.Parameter = request ?? new AppModule();
                var result = await _moduleService.Search(query);

                return DTResult(options, result, result.Response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SearchModules ");
            }
            return DTResult<AppModule>();
        }

        public async Task<IActionResult> SetModule(AppModule request)
        {
            return Ok(request.Id == Guid.Empty ?
                                await _moduleService.Insert(new Query<AppModule>(request)) :
                                await _moduleService.Update(new Query<AppModule>(request)));
        }



        public async Task<IActionResult> GetModule(AppModule request)
        {
            return Ok(await _moduleService.GetById(new Query<AppModule>(request)));
        }

        public async Task<IActionResult> DeleteModule(AppModule request)
        {
            return Ok(await _moduleService.Delete(new Query<AppModule>(request)));
        }

        public async Task<IActionResult> AddNewVersion(AppModuleLog request)
        {
            return Ok(await _moduleService.AddNewVersion(new Query<AppModuleLog>(request)));
        }
        public async Task<IActionResult> AddNewUpdate(AppModuleLog request)
        {
            return Ok(await _moduleService.AddNewUpdate(new Query<AppModuleLog>(request)));
        }
        public async Task<IActionResult> EditVersion(AppModuleLog request)
        {
            return Ok(await _moduleService.EditVersion(new Query<AppModuleLog>(request)));
        }

        public async Task<IActionResult> GetModuleLogs(AppModule request)
        {
            return Ok(await _moduleService.GetWihLogs(new Query<AppModule>(request)));
        }
    }
}
