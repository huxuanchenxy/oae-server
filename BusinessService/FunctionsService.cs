using AutoMapper;
using BusinessEntity;
using BusinessEntity.Sys;
using BusinessInterface;
using CommonUtility.Enums;
using DataBaseModels.Common;
using DataBaseModels.Sys;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService
{
    public class FunctionsService : BaseService, IFunctionsService
    {
        private readonly IMapper _mapper;
        public FunctionsService(ISqlSugarClient client, IMapper mapper) : base(client)
        {
            _mapper = mapper;
        }

        public List<FunctionsDto> GetFunctionsList(string lang="")
        {
            var list = _dbClient.Queryable<Functions>()
              .Where(d => d.Status == ConstantList.StautsValid)
              .WhereIF(!string.IsNullOrEmpty(lang),d=>d.Language==lang)
              .OrderBy(d=>d.Name)
              .ToList();
            var listDto = GetListForTree(list);
            return listDto;
        }

        public List<FunctionsDto> GetListForTree(List<Functions> lsDb, string parentId = "")
        {
            List<FunctionsDto> lsDto = new List<FunctionsDto>();
            var lsParent = lsDb.Where(x => x.ParentId == parentId);
            if (lsParent.Any())
            {
                foreach (var obj in lsParent)
                {
                    var dto = new FunctionsDto()
                    {
                        Id = obj.Id,
                        Name = obj.Name,
                        Content = obj.Content,
                        Desc = obj.Desc,
                        Meta = obj.Meta, 
                        ParentId = obj.ParentId,
                        Language = obj.Language,
                    };
                    dto.Children = GetListForTree(lsDb, obj.Id!);
                    lsDto.Add(dto);
                }
            }
            return lsDto;
        }

        public void SaveFunctions(FunctionsDto dto)
        {
            var dbObj = _mapper.Map<Functions>(dto); 
            if(string.IsNullOrEmpty(dbObj.Id)) dbObj.Id=Guid.NewGuid().ToString();
            Functions t = this.Insert(dbObj); 
        }

    }
}
