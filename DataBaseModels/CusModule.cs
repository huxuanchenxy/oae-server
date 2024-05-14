using DataBaseModels.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModels
{
    public  class CusModule: BaseModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public int Id { get; set; } 
        [SugarColumn(IsNullable = true)]
        public string? Name { get; set; }
        [SugarColumn(IsNullable = true)]
        public string? NameSpace { get; set; } 
       [SugarColumn(IsNullable = true,ColumnDataType = "Text")]
        public string? Properties { get; set; }
        [SugarColumn(IsNullable = true )]
        public string? Type { get; set; }
        [SugarColumn(IsNullable = true, ColumnDataType = "Text")]
        public string? Algorithms { get; set; }
        [SugarColumn(IsNullable = true, ColumnDataType = "Text")]
        public string? Interface { get; set; }
        [SugarColumn(IsNullable = true, ColumnDataType = "Text")]
        public string? Ecc { get; set; }  
        [SugarColumn(InsertServerTime = true, UpdateServerTime=true)]
        public DateTime? UpdateDate { get; set; }
    }
}
