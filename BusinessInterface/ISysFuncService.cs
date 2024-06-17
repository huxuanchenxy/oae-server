using BusinessEntity;
using BusinessEntity.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessInterface
{
    public interface ISysFuncService : IBaseService
    {
        List<SysFuncDto> GetSysFuncList();
        void SaveSysFunc(SysFuncDto dto);
        void AddModule(SysFuncDto dto);
        bool ValidateModuleName(string name, int pid); 
        void DelModule(int id);
        bool Rename(string name, int id, int pid);
        List<TreeListDto> GetModule(int projId);
    }
}
