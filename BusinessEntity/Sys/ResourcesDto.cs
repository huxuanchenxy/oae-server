using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Sys
{
    public class ResourcesDto
    {
        public int Id { get; set; }
        public int ParentId { get; set; } = 0;
        public string? Name { get; set; }
        public string? Namespace { get; set; }
        public string? Images { get; set; }
        public string? Type { get; set; }
        public string? XmlContent { get; set; }
        public string? JsonContent { get; set; }
        public int Status { get; set; }
        public string? Version { get; set; } = "";
        public int VersionEnable { get; set; } = 0;
    }
}
