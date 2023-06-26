using DataTech.System.Versioning.Models.Common;
using DataTech.System.Versioning.Models.Domain;
using DataTech.System.Versioning.Models.Dto.System;
using DataTech.System.Versioning.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataTech.System.Versioning.Controllers
{
    [ApiController]
    public class PublicController : ControllerBase
    {
        private readonly ILogger<PublicController> _logger;
        private readonly ISystemService _systemService;
        public PublicController(ILogger<PublicController> logger, ISystemService systemService)
        {
            _logger = logger;
            _systemService = systemService;
        }

        [HttpPost("~/system-version")]
        public async Task<GetSystemVersionResponse> GetSystemVersion(GetSystemVersionRequest request)
        {
            var response = new GetSystemVersionResponse();
            try
            {
                var getResult = await _systemService.GetByName(new Query<GetSystemVersionRequest>(request));

                if (getResult.Result)
                {
                    var appSystem = getResult.Response;
                    appSystem.Logs ??= new List<AppSystemLog>();
                    appSystem.Modules ??= new List<AppModule>();

                    var systemResponse = new SystemResponse
                    {
                        Name = appSystem.Name,
                        CurrentRelease = $"{appSystem.ReleaseIndex}.{appSystem.Modules?.Sum(x => x.VersionIndex).ToString().PadLeft(3, '0')}.{appSystem.Modules?.Sum(x => x.EnhancementIndex).ToString().PadLeft(3, '0')}",
                        Releases = appSystem.Logs.OrderBy(x => x.ReleaseIndex).Select(x => new ReleaseLog
                        {
                            Release = x.ReleaseIndex.ToString(),
                            Details = x.Description,
                            Date = x.CreationTime.Date
                        }).ToList(),
                        Modules = new List<ModuleResponse>()
                    };

                    foreach (var appModule in appSystem.Modules)
                    {
                        appModule.Logs ??= new List<AppModuleLog>();

                        var module = new ModuleResponse
                        {
                            Name = appModule.Name,
                            CurrentVersion = $"{appModule.VersionIndex}.{appModule.EnhancementIndex.ToString().PadLeft(3, '0')}.{appModule.FixIndex.ToString().PadLeft(3, '0')}",
                            Versions = appModule.Logs.Where(x => x.EnhancementIndex == 0 && x.FixIndex == 0).OrderBy(x => x.VersionIndex).Select(x => new VersionLog
                            {
                                Version = $"{x.VersionIndex}.{x.EnhancementIndex.ToString().PadLeft(3, '0')}.{x.FixIndex.ToString().PadLeft(3, '0')}",
                                Details = x.Description,
                                Date = x.CreationTime.Date,
                                Enhancements = appModule.Logs
                                                .Where(y => y.VersionIndex == x.VersionIndex && y.EnhancementIndex > 0 && y.FixIndex == 0)
                                                .OrderBy(y => y.EnhancementIndex)
                                                .Select(y => new EnhancementResponse
                                                {
                                                    Version = $"{y.VersionIndex}.{y.EnhancementIndex.ToString().PadLeft(3, '0')}.{y.FixIndex.ToString().PadLeft(3, '0')}",
                                                    Details = y.Description,
                                                    Date = y.CreationTime.Date,
                                                    Fixes = appModule.Logs
                                    .Where(z => z.VersionIndex == x.VersionIndex && z.EnhancementIndex == y.EnhancementIndex && z.FixIndex > 0)
                                    .OrderBy(z => z.VersionIndex)
                                    .Select(z => new FixResponse
                                    {
                                        Version = $"{z.VersionIndex}.{z.EnhancementIndex.ToString().PadLeft(3, '0')}.{z.FixIndex.ToString().PadLeft(3, '0')}",
                                        Details = z.Description,
                                        Date = z.CreationTime.Date
                                    }).ToList()
                                                }).ToList()
                            }).ToList()
                        };

                        systemResponse.Modules.Add(module);
                    }

                    response.Response = systemResponse;
                    response.Result = true;
                }
                else
                {
                    response.Message = getResult.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetSystemVersion Error @{Reqeust}", request);
                response.Message = ex.Message;
            }

            return response;
        }

    }
}
