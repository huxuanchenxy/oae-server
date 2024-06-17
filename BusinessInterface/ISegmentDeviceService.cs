using BusinessEntity.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessInterface
{
    public interface ISegmentDeviceService : IBaseService
    {
        List<SegmentDeviceDto> GetList(int segid);
        void Saves(List<SegmentDeviceDto> list);
        List<SegmentDeviceDto> GetAllList();
        bool UpdateStatus(int id);
    }
}
