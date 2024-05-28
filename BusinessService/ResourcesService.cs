using AutoMapper;
using BusinessEntity.Sys;
using BusinessInterface;
using CommonUtility.Enums;
using DataBaseModels.Sys;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService
{
    public class ResourcesService : BaseService, IResourcesService
    {
        private readonly IMapper _mapper;
        public ResourcesService(ISqlSugarClient client, IMapper mapper) : base(client)
        {
            _mapper = mapper;
        }

        public List<ResourcesDto> GetList()
        {
            var list = _dbClient.Queryable<Resources>()
            .Where(d => d.Status == ConstantList.StautsValid).ToList();
            var listDto = _mapper.Map<List<ResourcesDto>>(list);
            return listDto;
        }

        public void Saves(ResourcesDto dto)
        {
            var dbObj = _mapper.Map<Resources>(dto);
            if (dbObj.Id == 0)
            {
                this.Insert(dbObj);
            }
            else
            {
                this.Update(dbObj);
            }
        }

        public ResourcesDto? GetObj(string name, string version, int parentId)
        {
            var dbObj = _dbClient.Queryable<Resources>().Where(x => x.Status == ConstantList.StautsValid && x.Name == name && x.Version == version && x.ParentId == parentId).First();
            if (dbObj != null)
            {
                ResourcesDto dto = _mapper.Map<ResourcesDto>(dbObj);
                return dto;
            }
            else
            {
                return null;
            }
        }

   

        public bool UpdateStatus(int id)
        {
            var iCount = _dbClient.Updateable<Resources>()
            .SetColumns(it => new Resources() { Status = ConstantList.StautsDel })
            .Where(x => x.Id == id || x.ParentId == id).ExecuteCommand();
            return iCount > 0;
        }

    }
}
