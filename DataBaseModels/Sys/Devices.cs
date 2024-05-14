using DataBaseModels.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModels.Sys
{
    public class Devices : DataBaseIntId
    {
        public int ParentId { get; set; } = 0;
        public string? Name { get; set; } 
        public string? Namespace { get; set; }
        public string? Images { get; set; }
        public string? Type { get; set; }
        [SugarColumn(IsNullable = true, ColumnDataType = "Text")]
        public string? XmlContent { get; set; }
        [SugarColumn(IsNullable = true, ColumnDataType = "Text")]
        public string? JsonContent { get; set; } 

    }
}
