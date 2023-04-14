using DataTech.System.Versioning.Models.DataTable;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;
using System.Linq;

namespace DataTech.System.Versioning.Extensions
{
    public static class DataTableExtensions
    {

        public static DataTableOptions GetDataTableOptions(this HttpRequest request)
        {
            DataTableOptions dataTableOptions = new DataTableOptions();
            dataTableOptions.Draw = request.Form["draw"].FirstOrDefault();
            string text = request.Form["start"].FirstOrDefault();
            string text2 = request.Form["length"].FirstOrDefault();
            dataTableOptions.PageSize = ((text2 == null) ? 1 : Convert.ToInt32(text2));
            dataTableOptions.PageSize = Math.Max(1, dataTableOptions.PageSize);
            dataTableOptions.Start = ((text != null) ? Convert.ToInt32(text) : 0);
            dataTableOptions.PageIndex = (int)Math.Ceiling((decimal)(dataTableOptions.Start / dataTableOptions.PageSize));
            dataTableOptions.SortColumns = new List<DataTableSortColumn>();
            for (int i = 0; i < 6 && request.Form[$"order[{i}][column]"].FirstOrDefault() != null; i++)
            {
                DataTableSortColumn item = new DataTableSortColumn
                {
                    Name = request.Form["columns[" + request.Form[$"order[{i}][column]"].FirstOrDefault() + "][name]"].FirstOrDefault(),
                    Direction = request.Form[$"order[{i}][dir]"].FirstOrDefault(),
                    Order = i
                };
                dataTableOptions.SortColumns.Add(item);
            }

            dataTableOptions.Search = request.Form["search[value]"].FirstOrDefault();
            return dataTableOptions;
        }
    }
}
