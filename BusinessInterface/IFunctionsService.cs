using BusinessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessInterface
{
    public interface IFunctionsService : IBaseService
    {
        List<FunctionsDto> GetFunctionsList(string lang = "");
        void SaveFunctions(FunctionsDto dto);
    }
}
