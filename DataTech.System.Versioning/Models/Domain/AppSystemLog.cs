using System;

namespace DataTech.System.Versioning.Models.Domain
{
    public class AppSystemLog : BaseEntity<Guid>
    {
        public Guid AppSystemId { get; set; } 
        public int ReleaseIndex { get; set; } 
        public string Description { get; set; }
    }
}
