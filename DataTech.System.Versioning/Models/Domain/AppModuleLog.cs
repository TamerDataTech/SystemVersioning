using System;

namespace DataTech.System.Versioning.Models.Domain
{
    public class AppModuleLog : BaseEntity<Guid>
    {
        public Guid AppModuleId { get; set; }
        public int VersionIndex { get; set; }
        public int UpdateIndex { get; set; }
        public string Description { get; set; }
    }
}
