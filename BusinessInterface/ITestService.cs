using BusinessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessInterface
{
    public  interface ITestService:IBaseService
    {
        void Save(TestEntity req);
    }
}
