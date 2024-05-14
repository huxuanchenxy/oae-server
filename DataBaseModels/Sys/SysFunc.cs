using DataBaseModels.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModels.Sys
{
    public class SysFunc : DataBaseIntId
    { 
        public string? FuncName { get; set; }
        [SugarColumn(IsNullable = true)]
        public string? FuncUrl { get; set; }
        [SugarColumn(IsNullable = true)]
        public int? FuncParentId { get; set; }
        [SugarColumn(IsNullable = true)]
        public int? FuncLevelId { get; set; }
        [SugarColumn(IsNullable = true)]
        public string? Operation { get; set; }
    }
}
