using System;

namespace DataTech.System.Versioning.Models.Database.Request
{
    public class BackupTablesRequest
    {
        public string SourceDatabase { get; set; } 
        public bool LookupTablesOnly { get; set; } 
        public DateTime  DataFrom { get; set; }
    }
}
