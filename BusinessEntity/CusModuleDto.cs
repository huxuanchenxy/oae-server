using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class CusModuleDto
    {
        public int ? Id { get; set; }  
        public string? Name { get; set; }
        public string? NameSpace { get; set; } 
        public string? Type { get; set; } 
        public string? Properties { get; set; }
        public string? Algorithms { get; set; }
        public string? Interface { get; set; }
        public string? Ecc { get; set; }
       // public string? UpdateDate { get; set; } 
    }
}
