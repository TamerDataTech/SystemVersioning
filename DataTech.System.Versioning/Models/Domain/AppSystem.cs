using System;
using System.Collections.Generic;

namespace DataTech.System.Versioning.Models.Domain
{
    public class AppSystem : BaseEntity<Guid>
    { 
        public string Name { get; set; }
        public int ReleaseIndex { get; set; }


        public List<AppModule> Modules { get; set; }
        public List<AppSystemLog> Logs { get; set; }


    }
}
