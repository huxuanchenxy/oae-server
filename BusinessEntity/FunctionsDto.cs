using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class FunctionsDto
    {
        public string?  Id { get; set; }
        public string? Name { get; set; }
        public string? ParentId { get; set; }
        public string? Language { get; set; }
        //public string? Type { get; set; }
        public string? Desc { get; set; }
        public string? Meta { get; set; }
        public string? Content { get; set; }
        public List<FunctionsDto>? Children { get; set; }
    }
}
