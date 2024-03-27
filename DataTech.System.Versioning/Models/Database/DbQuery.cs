
using DataTech.System.Versioning.Models.Common;

namespace DatabaseUtils.Model.Database
{
    public class DbQuery<T> : Query<T> 
    { 
        public DbConnectionString Conn { get; set; }
    }
}
