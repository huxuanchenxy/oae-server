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
    public class ResourceFuncsService : BaseService, IResourceFuncsService
    {
        private readonly IMapper _mapper;
        public ResourceFuncsService(ISqlSugarClient client, IMapper mapper) : base(client)
        {
            _mapper = mapper;
        }

        public List<ResourceFuncsDto> GetList()
        {
            var list = _dbClient.Queryable<ResourceFuncs>()
            .Where(d => d.Status == ConstantList.StautsValid).ToList();
            var listDto = _mapper.Map<List<ResourceFuncsDto>>(list);
            return listDto;
        }

        public void Saves(ResourceFuncsDto dto)
        {
            var dbObj = _mapper.Map<ResourceFuncs>(dto);
            if (dbObj.Id == 0)
            {
                this.Insert(dbObj);
            }
            else
            {
                this.Update(dbObj);
            }
        }

       
    }
}
