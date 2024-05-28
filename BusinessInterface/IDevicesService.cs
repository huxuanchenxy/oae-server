using BusinessEntity.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessInterface
{
    public interface IDevicesService : IBaseService
    {
        List<DevicesDto> GetList();
        bool ValidateName(string name, int pid);
        void SaveDevices(DevicesDto dto);
        bool UpdateImage(string imageName, int id);
        DevicesDto? GetObj(string name, string version, int parentId);
        DevicesDto? GetObj(string name, int parentId);
        bool UpdateName(string name, int id);
        bool UpdateStatus(int id);
    }
}
