using AutoMapper;
using BusinessEntity;
using BusinessEntity.Sys;
using BusinessInterface;
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
    public class CusModuleService : BaseService, ICusModuleService
    {
        private readonly IMapper _mapper;
        public CusModuleService(ISqlSugarClient client, IMapper mapper) : base(client)
        {
            _mapper = mapper;
        }

        public CusModuleDto GetModule(int id)
        {
            var dbObj = this.Find<CusModule>(id);
            var dto = _mapper.Map<CusModuleDto>(dbObj);
            //dto.Interface = dto.Interface.Trim('\"');
            return dto;
        }

       
        public int  SaveModule(List<CusModuleDto> dtoList)
        {
            var dblIST = _mapper.Map<List<CusModule>>(dtoList);
            var iCount=_dbClient.Storageable(dblIST).ExecuteCommand();
            return iCount;
        }


    }
}
