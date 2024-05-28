using BusinessEntity.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessInterface
{
    public interface ISegmentsService : IBaseService
    {
        List<SegmentsDto> GetSegmentsList();
        bool ValidateName(string name, int pid);
        void SaveSegments(SegmentsDto dto);
        bool UpdateImage(string imageName, int id);
        SegmentsDto? GetObj(string name, string version, int parentId);
        SegmentsDto? GetObj(string name, int parentId);
        bool UpdateName(string name, int id);
        bool UpdateStatus(int id);
    }
}
