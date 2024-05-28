using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Sys
{
    public class SegmentDeviceDto
    {
        public int Id { get; set; }    
        public int SegmentId { get; set; }
        public int DeviceId { get; set; }
        public string DeviceName { get; set; } = "";
        public int Status { get; set; }
        public string? Images { get; set; } = "";
        public string? JsonContent { get; set; } = "";
        public string? Version { get; set; } = "";
        public int VersionEnable { get; set; } = 0;
    }
}
