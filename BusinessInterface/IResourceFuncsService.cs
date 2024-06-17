using BusinessEntity.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessInterface
{
    public interface IResourceFuncsService : IBaseService
    {
        List<ResourceFuncsDto> GetList();

        void Saves(ResourceFuncsDto req);
        bool UpdateStatus(int id);
    }
}
