using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Other
{
    public class TempJsonFuncDto
    {
        public string? caption { get; set; }
        public string? header { get; set; }
        public string? zh { get; set; }
        public string? en { get; set; }
        public string? snippet { get; set; }
        public string? meta { get; set; }
        List<TempJsonFuncDto>? children { get; set; }
    }
}
 
