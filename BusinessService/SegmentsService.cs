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
    public class SegmentsService : BaseService, ISegmentsService
    {
        private readonly IMapper _mapper;
        public SegmentsService(ISqlSugarClient client, IMapper mapper) : base(client)
        {
            _mapper = mapper;
        }

        public List<SegmentsDto> GetSegmentsList()
        {
            var list = _dbClient.Queryable<Segments>()
              .Where(d => d.Status == ConstantList.StautsValid).ToList();
            var listDto = _mapper.Map<List<SegmentsDto>>(list);
            return listDto;
        }

 

        public bool ValidateName(string name, int pid)
        {
            var pFlag = this.Query<Segments>(x => x.Name == name && x.ParentId == pid && x.Status == ConstantList.StautsValid).Any();
            return !pFlag;
        }

        public void SaveSegments(SegmentsDto dto)
        {
            var dbObj = _mapper.Map<Segments>(dto);
            if (dbObj.Id == 0)
            { 
                 this.Insert(dbObj); 
            }
            else{
                this.Update(dbObj);
            }
        }

        public bool UpdateImage(string imageName, int id)
        {
            var iCount = _dbClient.Updateable<Segments>()
            .SetColumns(it => new Segments() { Images = imageName })
            .Where(x => x.Id == id).ExecuteCommand();
            return iCount > 0;
        }

        public SegmentsDto? GetObj(string name, int parentId)
        {
            var dbObj = _dbClient.Queryable<Segments>().Where(x => x.Status == ConstantList.StautsValid && x.Name == name && x.ParentId != parentId).First();
            if (dbObj != null)
            {
                SegmentsDto dto = _mapper.Map<SegmentsDto>(dbObj);
                return dto;
            }
            else
            {
                return null;
            }
        }

        public SegmentsDto? GetObj(string name, string version, int parentId)
        {
            var dbObj = _dbClient.Queryable<Segments>().Where(x => x.Status == ConstantList.StautsValid && x.Name == name && x.Version == version && x.ParentId == parentId).First();
            if (dbObj != null)
            {
                SegmentsDto dto = _mapper.Map<SegmentsDto>(dbObj);
                return dto;
            }
            else
            {
                return null;
            }
        }

        public bool UpdateName(string name, int id)
        {
            var iCount = _dbClient.Updateable<Segments>()
            .SetColumns(it => new Segments() { Name = name })
            .Where(x => x.Id == id).ExecuteCommand();
            return iCount > 0;
        }

        public bool UpdateStatus(int id)
        {
            var iCount = _dbClient.Updateable<Segments>()
            .SetColumns(it => new Segments() { Status = ConstantList.StautsDel })
            .Where(x => x.Id == id || x.ParentId == id).ExecuteCommand();
            return iCount > 0;
        }
    }
}
