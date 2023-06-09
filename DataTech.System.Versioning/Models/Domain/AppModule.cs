﻿using System;
using System.Collections.Generic;

namespace DataTech.System.Versioning.Models.Domain
{
    public class AppModule : BaseEntity<Guid>
    {
        public Guid AppSystemId { get; set; }
        public string Name { get; set; } 
        public int ReleaseIndex { get; set; }
        public int VersionIndex { get; set; }
        public int EnhancementIndex { get; set; }
        public int FixIndex { get; set; }
        public List<AppModuleLog> Logs { get; set; }
    }
}
