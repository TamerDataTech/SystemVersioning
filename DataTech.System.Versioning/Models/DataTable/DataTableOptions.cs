using System.Collections.Generic;

namespace DataTech.System.Versioning.Models.DataTable
{
    public class DataTableOptions
    {
        public int Start { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public string Search { get; set; }

        public List<DataTableSortColumn> SortColumns { get; set; }

        public object Draw { get; set; }
    }

    public class DataTableSortColumn
    {
        public string Name { get; set; }

        public string Direction { get; set; }

        public int Order { get; set; }
    }
}
