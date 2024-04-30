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
    }
}
