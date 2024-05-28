using DataBaseModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModels.Sys
{
    public  class SegmentDevice : DataBaseIntId
    {
        public int SegmentId { get; set; }
        public int DeviceId { get; set; }
       
    }
}
