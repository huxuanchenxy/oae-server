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
    public class ControlsService : BaseService, IControlsService
    {
        private readonly IMapper _mapper;
        public ControlsService(ISqlSugarClient client, IMapper mapper) : base(client)
        {
            _mapper = mapper;
        }

        public List<ControlsDto> GetList()
        {
            var list = _dbClient.Queryable<Controls>()
            .Where(d => d.Status == ConstantList.StautsValid).ToList();
            var listDto = _mapper.Map<List<ControlsDto>>(list);
            return listDto;
        }

        public ControlsDto? GetObj(string name, int parentId)
        {
            var dbObj = _dbClient.Queryable<Controls>().Where(x => x.Status == ConstantList.StautsValid && x.Name == name  && x.ParentId != parentId).First();
            if (dbObj != null)
            {
                ControlsDto dto = _mapper.Map<ControlsDto>(dbObj);
                return dto;
            }
            else
            {
                return null;
            }
        }

        public ControlsDto? GetObj(string name, string version, int parentId)
        {
            var dbObj = _dbClient.Queryable<Controls>().Where(x => x.Status == ConstantList.StautsValid && x.Name == name && x.Version == version && x.ParentId == parentId).First();
            if (dbObj != null)
            {
                ControlsDto dto = _mapper.Map<ControlsDto>(dbObj);
                return dto;
            }
            else
            {
                return null;
            }
        }

        public void Saves(ControlsDto dto)
        {
            var dbObj = _mapper.Map<Controls>(dto);
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
            var pFlag = this.Query<Controls>(x => x.Name == name && x.ParentId == pid && x.Status == ConstantList.StautsValid).Any();
            return !pFlag;
        }


        public bool UpdateImage(string imageName, int id)
        {
            var iCount = _dbClient.Updateable<Controls>()
            .SetColumns(it => new Controls() { Images = imageName })
            .Where(x => x.Id == id).ExecuteCommand();
            return iCount > 0;
        }

        public bool UpdateName(string name, int id)
        {
            var iCount = _dbClient.Updateable<Controls>()
            .SetColumns(it => new Controls() { Name = name })
            .Where(x => x.Id == id).ExecuteCommand();
            return iCount > 0;
        }

        public bool UpdateStatus( int id)
        {
            var iCount = _dbClient.Updateable<Controls>()
            .SetColumns(it => new Controls() { Status = ConstantList.StautsDel })
            .Where(x => x.Id == id || x.ParentId==id).ExecuteCommand();
            return iCount > 0;
        }
    }
}
