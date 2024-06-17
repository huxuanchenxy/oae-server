using AutoMapper;
using BusinessEntity;
using BusinessEntity.Sys;
using BusinessInterface;
using CommonUtility.Enums;
using DataBaseModels;
using DataBaseModels.Sys;
using NetTaste;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService
{
    public class DevicesService : BaseService, IDevicesService
    {
        private readonly IMapper _mapper;
        public DevicesService(ISqlSugarClient client, IMapper mapper) : base(client)
        {
            _mapper = mapper;
        }

        public List<DevicesDto> GetList()
        {
            var list = _dbClient.Queryable<Devices>()
              .Where(d => d.Status == ConstantList.StautsValid).ToList();
            var listDto = _mapper.Map<List<DevicesDto>>(list);
            return listDto;
        }



        public bool ValidateName(string name, int pid)
        {
            var pFlag = this.Query<Devices>(x => x.Name == name && x.ParentId == pid && x.Status == ConstantList.StautsValid).Any();
            return !pFlag;
        }

        public void SaveDevices(DevicesDto dto)
        {
            var dbObj = _mapper.Map<Devices>(dto);
            if (dbObj.Id == 0)
            {
                this.Insert(dbObj);
            }
            else
            {
                this.Update(dbObj);
            }
        }

        public bool UpdateImage(string imageName, int id)
        {
            var iCount = _dbClient.Updateable<Devices>()
            .SetColumns(it => new Devices() { Images = imageName })
            .Where(x => x.Id == id).ExecuteCommand();
            return iCount > 0;
        }

        public DevicesDto? GetObj(string name, int parentId)
        {
            var dbObj = _dbClient.Queryable<Devices>().Where(x => x.Status == ConstantList.StautsValid && x.Name == name && x.ParentId != parentId).First();
            if (dbObj != null)
            {
                DevicesDto dto = _mapper.Map<DevicesDto>(dbObj);
                return dto;
            }
            else
            {
                return null;
            }
        }

        public DevicesDto? GetObj(string name, string version, int parentId)
        {
            var dbObj = _dbClient.Queryable<Devices>().Where(x => x.Status==ConstantList.StautsValid && x.Name == name && x.Version == version && x.ParentId == parentId).First();
            if (dbObj != null)
            {
                DevicesDto dto = _mapper.Map<DevicesDto>(dbObj);
                return dto;
            }
            else
            {
                return null;
            }
        }

        public bool UpdateName(string name, int id)
        {
            var iCount = _dbClient.Updateable<Devices>()
            .SetColumns(it => new Devices() { Name = name })
            .Where(x => x.Id == id).ExecuteCommand();
            return iCount > 0;
        }

        public bool UpdateStatus(int id)
        {
            var iCount = _dbClient.Updateable<Devices>()
            .SetColumns(it => new Devices() { Status = ConstantList.StautsDel })
            .Where(x => x.Id == id || x.ParentId == id).ExecuteCommand();
            return iCount > 0;
        }
    }
}
