using AutoMapper;
using BusinessEntity;
using BusinessEntity.Other;
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
            var listRouterId = listDto.Where(x => !string.IsNullOrEmpty(x.FuncUrl) && (x.FuncUrl.Contains(":id") || x.FuncUrl.Contains(":pid")));
            if (listRouterId.Any())
            {
                foreach (var item in listRouterId)
                {
                    item.FuncUrl = item.FuncUrl!.Replace(":id", item.Id.ToString()).Replace(":pid", item.FuncParentId.ToString()); 
                }
            }
            var iList = list.Where(x => x.OperationType== "module" && x.Status == ConstantList.StautsValid).Select(x => x.Id).ToList();
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

                    var interfaceObj = AssembleSysFunc("接口", "interface", model.Id, funcLevelId, parentDto.FuncUrl!, idIndex++,"", "");
                    listDto.Add(interfaceObj);

                    var algoObj = AssembleSysFunc("算法", "algorithm", model.Id, funcLevelId, "", idIndex++, "right,new", "algorithm");
                    listDto.Add(algoObj);

                    var eccObj = AssembleSysFunc("ECC", "ecc", model.Id, funcLevelId, parentDto.FuncUrl!, idIndex++, "", "");
                    listDto.Add(eccObj);

                    if (model.Algorithms?.Length > 0)
                    {
                        List<AlgorithmsDto> listSfDto = JsonSerializer.Deserialize<List<AlgorithmsDto>>(model.Algorithms)!; //(model.Algorithms!.TrimEnd('\"').TrimStart('\"'))!;
                        if (listSfDto?.Count > 0)
                        {
                            foreach (var sfDto in listSfDto)
                            {
                                var algoChildObj = AssembleSysFunc(sfDto.text!, $"algorithm/{sfDto.text}", algoObj.Id, funcLevelId + 1, parentDto.FuncUrl!, idIndex++,"right,del,open,rename", "algorithm");
                                algoChildObj.Type = sfDto.type;
                                listDto.Add(algoChildObj);
                            }
                        }
                    }
                }
            }
            else
            { 
                foreach (var item in listDto)
                {
                    item.FuncUrl = $"{item.FuncUrl}/{item.FuncParentId}/{item.Id}";
                }
            }
            return listDto;
        }

        public SysFuncDto AssembleSysFunc(string name, string nameEn, int? id, int funcLevelId, string funcUrl, int? newId, string operation, string operationType)
        {
            SysFuncDto newobj = new SysFuncDto()
            {
                Id = newId,
                FuncName = name,
                FuncLevelId = funcLevelId,
                FuncUrl = string.IsNullOrEmpty(funcUrl) ? "" : $"{funcUrl}/{nameEn}",
                FuncParentId = id,
                Operation = operation,
                OperationType = operationType
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

        public void  AddModule(SysFuncDto dto)
        {
            var dbObj = _mapper.Map<SysFunc>(dto);
            if (dbObj.Id == 0)
            {
                //dto.Id= dbObj.Id = Guid.NewGuid().ToString();
                var newFunc = this.Insert(dbObj);
                var moduleId = newFunc.Id;
                CusModule newModule = new CusModule() { Id = moduleId };
                newModule.Interface = "{\"input_events\":[],\"output_events\":[],\"inputs\":[],\"outputs\":[],\"internals\":[],\"temps\":[]}";
                this.Insert(newModule);               

            }
        }

        public void DelModule(int id)
        { 
            _dbClient.Updateable<SysFunc>()
                  .SetColumns(it=>new SysFunc() { Status=ConstantList.StautsDel})
                  .Where(x => x.Id == id).ExecuteCommand(); ;
            _dbClient.Updateable<CusModule>()
                 .SetColumns(it => new CusModule() { Status = ConstantList.StautsDel })
              .Where(x => x.Id == id).ExecuteCommand(); ;
        }

        public bool ValidateModuleName(string name, int pid)
        {
            var pFlag = this.Query<SysFunc>(x => x.FuncName == name && x.FuncParentId == pid &&x.Status==ConstantList.StautsValid).Any();
            return !pFlag;
        }

        public bool Rename(string name, int id,int pid)
        {
            var pFlag = this.Query<SysFunc>(x => x.FuncName == name && x.FuncParentId == pid && x.Id !=id).Any();
            if(!pFlag)
            {
                var iCount=_dbClient.Updateable<SysFunc>()
                .SetColumns(it => new SysFunc() { FuncName = name})
                .Where(x => x.Id == id).ExecuteCommand();
                pFlag = iCount > 0;
            }
            else
            {
                pFlag = !pFlag;
            }
            return pFlag;
        }

        public List<TreeListDto> GetModule(int projId)
        {
            // var objFunc = _dbClient.Queryable<SysFunc>()
            // .Where(d => d.Status == ConstantList.StautsValid && d.FuncParentId==int.Parse(projId) && d.OperationType=="func").First();
            // if(objFunc != null)
            // {
            //     var objBfb=_dbClient.Queryable<SysFunc>()
            //.Where(d => d.Status == ConstantList.StautsValid && d.FuncParentId ==objFunc.Id && d.OperationType == "bfb").First();
            //     if (objBfb != null)
            //     {
            //         var objBfb = _dbClient.Queryable<SysFunc>()
            //    .Where(d => d.Status == ConstantList.StautsValid && d.FuncParentId == objFunc.Id && d.OperationType == "bfb").First();
            //     }
            //// } 
            //     var f= _dbClient.Queryable<SysFunc>().Where(x => (x.FuncParentId == SqlFunc.Subqueryable<SysFunc>().Where(d =>
            // d.Status == ConstantList.StautsValid && d.FuncParentId == 2 && d.OperationType == "func"
            //).Select(d => d.Id)) &&x.OperationType=="bfb").First();

            var lsModuleIds = _dbClient.Queryable<SysFunc>()
                 .Where(m => (m.FuncParentId == SqlFunc.Subqueryable<SysFunc>()
                 .Where(x => (x.FuncParentId == SqlFunc.Subqueryable<SysFunc>().Where(d =>
             d.Status == ConstantList.StautsValid && d.FuncParentId == projId && d.OperationType == "func"
            ).Select(d => d.Id)) && x.OperationType == "bfb").Select(x => x.Id))
            && m.Status == ConstantList.StautsValid && m.OperationType == "module").Select(m => m.Id).ToList();

            var moduleList = _dbClient.Queryable<CusModule>()
                .LeftJoin<SysFunc>((c,s)=>c.Id == s.Id)
                .Where((c,s) => s.Status == ConstantList.StautsValid).In(c => c.Id, lsModuleIds)
                .Select((c,s)=>new TreeListDto()
                {
                    Id=c.Id,
                    Name=s.FuncName,
                    ParentId=int.Parse(s.FuncParentId.ToString()!),
                    JsonContent=c.Interface,
                    Images="",
                    Type=""
                })
                .ToList();
            return moduleList; 
        }
    }
}
