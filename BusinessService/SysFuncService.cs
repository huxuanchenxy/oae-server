using AutoMapper;
using BusinessEntity.Other;
using BusinessEntity.Sys;
using BusinessInterface;
using CommonUtility.Enums;
using DataBaseModels;
using DataBaseModels.Sys;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace BusinessService
{
    public class SysFuncService : BaseService, ISysFuncService
    {
        private readonly IMapper _mapper;
        public SysFuncService(ISqlSugarClient client, IMapper mapper) : base(client)
        {
            _mapper = mapper;
        }

        public List<SysFuncDto> GetSysFuncList()
        {
            var list = _dbClient.Queryable<SysFunc>()
              .Where(d => d.Status == ConstantList.StautsValid).ToList();
            var listDto = _mapper.Map<List<SysFuncDto>>(list);
            var iList = list.Where(x => x.FuncLevelId == 3 && x.Status == ConstantList.StautsValid).Select(x => x.Id).ToList();
            var moduleList = _dbClient.Queryable<CusModule>().Where(d => d.Status == ConstantList.StautsValid).In(it => it.Id, iList).ToList();

            if (moduleList.Any())
            {
                int? idIndex = listDto.Max(x => x.Id) + 1;
                foreach (var model in moduleList)
                {
                    var parentDto = listDto.First(x => x.Id == model.Id);
                    var parentId = parentDto.FuncParentId;
                    var funcLevelId = parentDto.FuncLevelId + 1;
                    parentDto.FuncUrl = $"{parentDto.FuncUrl}/{parentId}/{parentDto.Id}";

                    var interfaceObj = AssembleSysFunc("接口", "interface", model.Id, funcLevelId, parentDto.FuncUrl!, idIndex++);
                    listDto.Add(interfaceObj);

                    var algoObj = AssembleSysFunc("算法", "algorithm", model.Id, funcLevelId, "", idIndex++);
                    listDto.Add(algoObj);

                    var eccObj = AssembleSysFunc("ECC", "ecc", model.Id, funcLevelId, parentDto.FuncUrl!, idIndex++);
                    listDto.Add(eccObj);

                    if (model.Algorithms?.Length > 0)
                    {
                        List<AlgorithmsDto> listSfDto = JsonSerializer.Deserialize<List<AlgorithmsDto>>(model.Algorithms!.TrimEnd('\"').TrimStart('\"'))!;
                        if (listSfDto.Count > 0)
                        {
                            foreach (var sfDto in listSfDto)
                            {
                                var algoChildObj = AssembleSysFunc(sfDto.text!, $"algorithm/{sfDto.text}", algoObj.Id, funcLevelId + 1, parentDto.FuncUrl!, idIndex++);
                                listDto.Add(algoChildObj);
                            }
                        }
                    }
                }
            }
            else
            {
                var mList = listDto.Where(x => x.FuncLevelId == 3 );
                foreach (var item in mList)
                {
                    item.FuncUrl = $"{item.FuncUrl}/{item.FuncParentId}/{item.Id}";
                }
            }
            return listDto;
        }

        public SysFuncDto AssembleSysFunc(string name,string nameEn,int? id,int funcLevelId,string funcUrl,int? newId)
        {
            SysFuncDto newobj = new SysFuncDto()
            {
                Id = newId,
                FuncName = name,
                FuncLevelId = funcLevelId,
                FuncUrl =string.IsNullOrEmpty(funcUrl)?"":$"{funcUrl}/{nameEn}",
                FuncParentId=id,  
            };
            return newobj;
        }

        public void SaveSysFunc(SysFuncDto dto)
        {
            var dbObj = _mapper.Map<SysFunc>(dto);
            if (dbObj.Id == 0)
            {
                //dto.Id= dbObj.Id = Guid.NewGuid().ToString();
                this.Insert(dbObj);
            }
            else
            {
                _dbClient.Updateable(dbObj).IgnoreColumns(it => new { it.CreateDate }).ExecuteCommand();
            }
        }

    }
}
