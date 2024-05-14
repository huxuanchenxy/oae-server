using BusinessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessInterface
{
    public interface ICusModuleService : IBaseService
    {
        CusModuleDto GetModule(int id); 
        int SaveModule(List<CusModuleDto> dtoList);
    }
}
