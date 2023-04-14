using System;
using System.Collections.Generic;

namespace DataTech.System.Versioning.Models.Domain
{
    public class AppModule : BaseEntity<Guid>
    {
        public Guid AppSystemId { get; set; }
        public string Name { get; set; } 
        public int VersionIndex { get; set; }
        public int UpdateIndex { get; set; }
        public List<AppModuleLog> Logs { get; set; }
    }
}
