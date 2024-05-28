using BusinessEntity.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessInterface
{
    public interface IControlsService : IBaseService
    {
        List<ControlsDto> GetList();
        bool ValidateName(string name, int pid);
        void Saves(ControlsDto dto);
        bool UpdateImage(string imageName, int id);
        ControlsDto? GetObj(string name, string version, int parentId);
        ControlsDto? GetObj(string name, int parentId);
        bool UpdateName(string name, int id);
        bool UpdateStatus(int id);
    }
}
