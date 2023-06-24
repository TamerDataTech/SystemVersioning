using DataTech.System.Versioning.Models.Domain;
using System;
using System.Collections.Generic;

namespace DataTech.System.Versioning.Models.Dto.System
{
    public class GetSystemVersionResponse : BaseResponse<SystemResponse>
    {

    }

    public class SystemResponse
    {
        public string Name { get; set; } 
        public string CurrentRelease { get; set; }
        public List<ReleaseLog> Releases { get; set; }
        public List<ModuleResponse> Modules { get; set; }

    }


    public class ModuleResponse
    {
        public string Name { get; set; }
        public string CurrentVersion { get; set; }
        public List<VersionLog> Versions { get; set; }
    }

    public class UpdateResponse
    { 
        public string Update { get; set; }
        public string  Details { get; set; }
        public DateTime Date { get; set; }
    }


    public class ReleaseLog
    {
        public string Release { get; set; }
        public string Details { get; set; }
        public DateTime Date { get; set; }

    }

    public class VersionLog
    {
        public string Version { get; set; }
        public string Details { get; set; }
        public DateTime Date { get; set; }
        public List<UpdateResponse> Updates { get; set; }

    }
}
