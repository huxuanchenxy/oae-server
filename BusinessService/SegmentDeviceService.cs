using AutoMapper;
using BusinessEntity.Sys;
using BusinessInterface;
using CommonUtility.Enums;
using DataBaseModels.Sys;
using NetTaste;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService
{
    public class SegmentDeviceService : BaseService, ISegmentDeviceService
    {
        private readonly IMapper _mapper;
        public SegmentDeviceService(ISqlSugarClient client, IMapper mapper) : base(client)
        {
            _mapper = mapper;
        }

        public List<SegmentDeviceDto> GetList(int segid)
        {
            //var list = _dbClient.Queryable<SegmentDevice>()
            //.Where(d => d.Status == ConstantList.StautsValid).ToList();
            //var listDto = _mapper.Map<List<SegmentDeviceDto>>(list);
            //return listDto; 
            var list = _dbClient.Queryable<SegmentDevice>()
                             .LeftJoin<Devices>((sd, d) => sd.DeviceId == d.Id)
                             .LeftJoin<Segments>((sd, d, s) => sd.SegmentId == s.Id)
                             .Where((sd, d, s) => sd.SegmentId==segid && sd.Status == ConstantList.StautsValid && d.Status == ConstantList.StautsValid && s.Status == ConstantList.StautsValid)
                             .Select((sd, d, s) => new SegmentDeviceDto { Id = sd.Id, DeviceName = d.Name!, DeviceId = d.Id, Status = 1, SegmentId = s.Id })
                             .ToList();
            return list;
        }

        public List<SegmentDeviceDto> GetAllList()
        {
            //var list = _dbClient.Queryable<SegmentDevice>()
            //.Where(d => d.Status == ConstantList.StautsValid).ToList();
            //var listDto = _mapper.Map<List<SegmentDeviceDto>>(list);
            //return listDto; 
            var list = _dbClient.Queryable<SegmentDevice>()
                             .LeftJoin<Devices>((sd, d) => sd.DeviceId == d.Id)
                             .LeftJoin<Segments>((sd, d, s) => sd.SegmentId == s.Id)
                             .Where((sd, d, s) =>  sd.Status == ConstantList.StautsValid && d.Status == ConstantList.StautsValid && s.Status == ConstantList.StautsValid)
                             .Select((sd, d, s) => new SegmentDeviceDto { Id = sd.Id, DeviceName = d.Name!, DeviceId = d.Id, Images=d.Images,JsonContent=d.JsonContent, Status = 1, SegmentId = s.Id })
                             .ToList();
            return list;
        }

        public void Saves(List<SegmentDeviceDto> list)
        {
            foreach (var d in list)
            {
                var obj = _dbClient.Queryable<SegmentDevice>().Where(x => x.DeviceId == d.DeviceId
                && x.SegmentId == d.SegmentId && x.Status == ConstantList.StautsValid).First();
                if(obj == null)
                {
                    var newDbObj = _mapper.Map<SegmentDevice>(d);
                    this.Insert(newDbObj);
                } 
            } 
            //var lsObj = _mapper.Map<List<SegmentDevice>>(list); 
            //if (dbObj.Id == 0)
            //{
            //    this.Insert(dbObj);
            //}
            //else
            //{
            //    this.Update(dbObj);
            //}
        }


        public bool UpdateStatus(int id)
        {
            var iCount = _dbClient.Updateable<SegmentDevice>()
            .SetColumns(it => new SegmentDevice() { Status = ConstantList.StautsDel })
            .Where(x => x.Id == id).ExecuteCommand();
            return iCount > 0;
        }


    }
}
