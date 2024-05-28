using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Sys
{
    public  class TreeListDto
    {
        public int Id { get; set; }
        public int ParentId { get; set; } = 0;
        public string? Name { get; set; } 
        public string? Images { get; set; } = ""; 
        public string? JsonContent { get; set; } = "";
        public List<TreeListDto>? Children { get; set; }
        public string Type { get; set; } = "";
       
    }
}
