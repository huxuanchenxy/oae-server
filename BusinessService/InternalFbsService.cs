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
    public class InternalFbsService : BaseService, IInternalFbsService
    {
        private readonly IMapper _mapper;
        public InternalFbsService(ISqlSugarClient client, IMapper mapper) : base(client)
        {
            _mapper = mapper;
        }

        public List<InternalFbsDto> GetList()
        {
            var list = _dbClient.Queryable<InternalFbs>()
            .Where(d => d.Status == ConstantList.StautsValid).ToList();
            var listDto = _mapper.Map<List<InternalFbsDto>>(list);
            return listDto;
        }  

        public void Saves(InternalFbsDto dto)
        {
            var dbObj = _mapper.Map<InternalFbs>(dto);
            if (dbObj.Id == 0)
            {
                this.Insert(dbObj);
            }
            else
            {
                this.Update(dbObj);
            }
        }

        public bool ValidateName(string name, int pid)
        {
            var pFlag = this.Query<InternalFbs>(x => x.Name == name && x.ParentId == pid && x.Status == ConstantList.StautsValid).Any();
            return !pFlag;
        } 

        public bool UpdateName(string name, int id)
        {
            var iCount = _dbClient.Updateable<InternalFbs>()
            .SetColumns(it => new InternalFbs() { Name = name })
            .Where(x => x.Id == id).ExecuteCommand();
            return iCount > 0;
        }

        public bool UpdateStatus( int id)
        {
            var iCount = _dbClient.Updateable<InternalFbs>()
            .SetColumns(it => new InternalFbs() { Status = ConstantList.StautsDel })
            .Where(x => x.Id == id || x.ParentId==id).ExecuteCommand();
            return iCount > 0;
        }

        public InternalFbsDto? GetObj(string name, string version, int parentId)
        {
            var dbObj = _dbClient.Queryable<InternalFbs>().Where(x => x.Status == ConstantList.StautsValid && x.Name == name && x.Version == version && x.ParentId == parentId).First();
            if (dbObj != null)
            {
                InternalFbsDto dto = _mapper.Map<InternalFbsDto>(dbObj);
                return dto;
            }
            else
            {
                return null;
            }
        }

        public InternalFbsDto? GetObj(string name, int parentId)
        {
            var dbObj = _dbClient.Queryable<InternalFbs>().Where(x => x.Status == ConstantList.StautsValid && x.Name == name && x.ParentId != parentId).First();
            if (dbObj != null)
            {
                InternalFbsDto dto = _mapper.Map<InternalFbsDto>(dbObj);
                return dto;
            }
            else
            {
                return null;
            }
        }
    }
}
