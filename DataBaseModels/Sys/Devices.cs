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
        [SugarColumn(IsNullable = true)]
        public string? Namespace { get; set; } = "";
        [SugarColumn(IsNullable = true)]
        public string? Images { get; set; } = "";
        [SugarColumn(IsNullable = true)]
        public string? Type { get; set; } = "";
        [SugarColumn(IsNullable = true)]
        public string? Version { get; set; } = "";
        [SugarColumn(IsNullable = true)]
        public int VersionEnable { get; set; } = 0;
        [SugarColumn(IsNullable = true, ColumnDataType = "Text")]
        public string? XmlContent { get; set; }
        [SugarColumn(IsNullable = true, ColumnDataType = "Text")]
        public string? JsonContent { get; set; } 

    }
}
