using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DataTech.System.Versioning.Models.Common
{
    public class Query<T> : Query
    {
        public T Parameter { get; set; }

        public Query()
        {
        }

        public Query(T param)
        {
            Parameter = param;
        } 
    }

    public class Query
    {
        public Query()
        {
            PageSize = 10;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; }
        public OrderDir OrderDir { get; set; }
        public Guid UserId { get; set; }
        public string Culture { get; set; }
    }

    public enum OrderDir
    {
        ASC,
        DESC
    }
}
