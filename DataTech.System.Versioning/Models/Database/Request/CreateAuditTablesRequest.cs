using System.Collections.Generic;

namespace DataTech.System.Versioning.Models.Database.Request
{
    public class CreateAuditTablesRequest
    {
        public string SourceDatabase { get; set; } 
        public string TargetDatabase { get; set; }
        public bool CreateNewDatabase { get; set; }
        public string NewDatabaseName { get; set; }
        public List<string> Tables { get; set; } 
        public string Suffix { get; set; }

    }
}
