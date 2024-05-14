using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Sys
{
    public class SysFuncDto
    {
        public int? Id { get; set; }
        public string? FuncName { get; set; }
        public string? FuncUrl { get; set; }
        public int? FuncParentId { get; set; }
        public int FuncLevelId { get; set; }
        public bool? IsEdit { get;set; }=false;
        public string? Type { get; set; } = ""; 
        public string? Operation { get; set; }
    }
}
