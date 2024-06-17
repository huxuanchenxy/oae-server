using BusinessEntity.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessInterface
{
    public interface IInternalFbsService : IBaseService
    {
        List<InternalFbsDto> GetList();
        bool ValidateName(string name, int pid);
        void Saves(InternalFbsDto dto); 
        bool UpdateName(string name, int id);
        bool UpdateStatus(int id);
        InternalFbsDto? GetObj(string name, string version, int parentId); 
        InternalFbsDto? GetObj(string name, int parentId);
    }
}
