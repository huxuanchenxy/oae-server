using BusinessEntity.Other;
using BusinessEntity.Sys;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessInterface
{
    public interface IResourcesService : IBaseService
    {
        List<ResourcesDto> GetList(); 

        void Saves(ResourcesDto req);
        ResourcesDto? GetObj(string name, string version, int parentId);  
        bool UpdateStatus(int id);
    }
    
}
