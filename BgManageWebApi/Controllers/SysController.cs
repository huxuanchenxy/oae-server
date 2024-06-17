using BusinessEntity.Other;
using BusinessEntity;
using BusinessInterface;
using BusinessService;
using CommonUtility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessEntity.Sys;
using System.ComponentModel;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Xml;
using System.Dynamic;
using Newtonsoft.Json.Serialization;
using SqlSugar;
using System.Text.Json;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using NetTaste;
using DataBaseModels.Sys;
using System.Collections.Generic;
using AutoMapper;
using System.Security.Cryptography;
using Microsoft.VisualBasic;
using System.IO;
using SqlSugar.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BgManageWebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class SysController : ControllerBase
    {
        private readonly ISysFuncService _iSysFuncService;
        private readonly IDevicesService _iDevicesService;
        private readonly ISegmentsService _iSegmentsService;
        private readonly IControlsService _iControlsService;
        private readonly IResourcesService _iResourcesService;
        private readonly IResourceFuncsService _iResourceFuncsService;
        private readonly ISegmentDeviceService  _iSegmentDeviceService;
        private readonly IInternalFbsService _iInternalFbsService;
        private readonly IMapper _mapper;
        private IConfiguration _iConfig;
        public SysController(ISysFuncService iSysFuncService, IDevicesService iDevicesService,
            ISegmentsService iSegmentsService, IControlsService iControlsService ,
                                             IResourcesService iResourcesService, IMapper mapper,
                                             IConfiguration iConfig,
                                              IInternalFbsService iInternalFbsService,
                IResourceFuncsService iResourceFuncsService, ISegmentDeviceService iSegmentDeviceService)
        {
            _iSysFuncService = iSysFuncService;
            _iDevicesService = iDevicesService;
            _iSegmentsService = iSegmentsService;
            _iControlsService = iControlsService;
            _iResourcesService = iResourcesService;
            _iResourceFuncsService = iResourceFuncsService;
            _iSegmentDeviceService = iSegmentDeviceService;
            _mapper = mapper;
            _iConfig = iConfig;
            _iInternalFbsService = iInternalFbsService;
        }

        [HttpGet]
        public async Task<ApiResultDto> GetAllDeviceList()
        {
            List<TreeListDto> list = new List<TreeListDto>();

            //var devList = _iDevicesService.GetList();
            //var listTdev=_mapper.Map<List<TreeListDto>>(devList);
            //var parTDev= listTdev.Where(x=>x.ParentId==0).ToList();
            // HandleTree(parTDev, listTdev);
            //list.AddRange(parTDev);

            var controlList = _iControlsService.GetList();
            var listTCon = _mapper.Map<List<TreeListDto>>(controlList);
            var parTCon = listTCon.Where(x => x.ParentId == 0).ToList();
            HandleTree(parTCon, listTCon);
            list.AddRange(parTCon);


            //var resourceList = _iResourcesService.GetList();
            //var listTRes = _mapper.Map<List<TreeListDto>>(resourceList);
            //var parTRes = listTRes.Where(x => x.ParentId == 0).ToList();
            // HandleTree(parTRes, listTRes);
            //list.AddRange(parTRes);


            var segdevList=_iSegmentDeviceService.GetAllList();
            var devList = _iDevicesService.GetList();
            var segList = _iSegmentsService.GetSegmentsList();
            var listTSeg = _mapper.Map<List<TreeListDto>>(segList);
            var parTSeg= listTSeg.Where(x => x.ParentId == 0).ToList();
            HandleTreeSeg(parTSeg, listTSeg,segdevList);
            list.AddRange(parTSeg);

            return await ApiResult.Success(list);
        }

        private void  HandleTree(List<TreeListDto> ls, List<TreeListDto> AllList)
        {
            foreach (var item in ls)
            {
                var childrenList = AllList.Where(x => x.ParentId == item.Id).ToList();
                if (childrenList.Any())
                {
                    item.Children=childrenList.ToList();
                    HandleTree(childrenList, AllList);
                } 
            } 
        }

        private void HandleTreeSeg(List<TreeListDto> ls, List<TreeListDto> AllList, List<SegmentDeviceDto> segdevList)
        {
            foreach (var item in ls)
            {
                var childrenList = AllList.Where(x => x.ParentId == item.Id).ToList();
                if (childrenList.Any())
                {
                    item.Children = childrenList.ToList();
                    HandleTreeSeg(childrenList, AllList,segdevList);
                }
                else
                {
                    if (item.JsonContent!.Length > 0)
                    {
                        int segId = item.Id;
                        var devsegList=segdevList.Where(x=>x.SegmentId == segId).ToList(); 
                        if (devsegList.Any())
                        {
                            item.Children = new List<TreeListDto>();
                            TreeListDto newDto = new TreeListDto()
                            {
                                Id = -segId,
                                Name = "终端设备",
                                 Children = new List<TreeListDto>()
                            };
                            foreach (var devseg in devsegList)
                            {
                                TreeListDto newChildDto = new TreeListDto()
                                {
                                    Id = devseg.DeviceId,
                                    Images = devseg.Images,
                                    JsonContent = devseg.JsonContent,
                                    ParentId = newDto.Id,
                                    Name = devseg.DeviceName,
                                };
                                newDto.Children!.Add(newChildDto);
                            }
                            item.Children!.Add(newDto);
                        } 
                    }
                }
            }
        }
        
        [HttpGet]
        public async Task<ApiResultDto> GetTreeForApplication(string pid)
        {
            List<TreeListDto> list = new List<TreeListDto>();

            #region 逻辑处理功能块
            var interfbslList = _iInternalFbsService.GetList();
            var listTInter = _mapper.Map<List<TreeListDto>>(interfbslList);
            foreach (var item in listTInter)
            {
                item.Type = "generic";
            }
            var parInter = listTInter.Where(x => x.ParentId == 0).ToList();
            HandleTree(parInter, listTInter);
            list.AddRange(parInter);
            #endregion

            #region 项目功能块
            var iPid = int.Parse(pid);
           var listCusModule=  _iSysFuncService.GetModule(iPid); 
            foreach (var item in listCusModule)
            {
                item.Type = "project";
            }
            var cusModuleTree = new TreeListDto()
            {
                Id= iPid,
                ParentId=0,
                Name="项目功能块",
                Children = new List<TreeListDto>(),
                Images ="",
                Type= "project",
                JsonContent=""
            };
            cusModuleTree.Children.AddRange(listCusModule);
            list.Add(cusModuleTree);
            #endregion  
            return await ApiResult.Success(list);
        }

        #region 功能菜单
        /// <summary>
        /// 获取功能菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResultDto> GetSysFuncList()
        {
            var list=_iSysFuncService.GetSysFuncList(); 
            return await ApiResult.Success(list);
        }

        /// <summary>
        /// 新增功能菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto> AddSysFuncList(SysFuncDto req)
        {
            _iSysFuncService.SaveSysFunc(req);
            return await ApiResult.Success(true);
        }

        /// <summary>
        /// 新增功能菜单--模块
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto> AddModule(SysFuncDto req)
        {
            _iSysFuncService.AddModule(req);
            return await ApiResult.Success(true);
        }

        /// <summary>
        /// 删除功能菜单--模块
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResultDto> DelModule(int  id)
        {
            _iSysFuncService.DelModule(id);
            return await ApiResult.Success(true);
        }

        [HttpGet]
        public async Task<ApiResultDto> ValidateModuleName(string name, string pid = "0")
        {
            var pFlag = _iSysFuncService.ValidateModuleName(name, int.Parse(pid));
            return await ApiResult.Success(pFlag);
        } 

        [HttpGet]
        public async Task<ApiResultDto> Rename(string name, string id, string pid = "0")
        {
            var pFlag = _iSysFuncService.Rename(name, int.Parse(id),int.Parse(pid));
            return await ApiResult.Success(pFlag);
        }

        #endregion

        #region 设备 
        [HttpGet]
        public async Task<ApiResultDto> GetDevicesList()
        { 
            var list = _iDevicesService.GetList(); 
            return await ApiResult.Success(list);
        }

        [HttpGet]
        public async Task<ApiResultDto> ValidateDevicesName(string name, string pid = "0")
        {
            var pFlag = _iDevicesService.ValidateName(name, int.Parse(pid));
            return await ApiResult.Success(pFlag);
        }

        [HttpPost]
        public async Task<ApiResultDto> SaveDevices(DevicesDto req)
        {
            _iDevicesService.SaveDevices(req);
            return await ApiResult.Success(true);
        }

        [HttpGet]
        public async Task<ApiResultDto> UpdateDevicesName(string name, string id)
        {
            var pFlag = _iDevicesService.UpdateName(name, int.Parse(id));
            return await ApiResult.Success(pFlag);
        }
        [HttpGet]
        public async Task<ApiResultDto> UpdateDevicesStatus(string id)
        {
            var pFlag = _iDevicesService.UpdateStatus(int.Parse(id));
            return await ApiResult.Success(pFlag);
        }
        #endregion

        #region 网络段
        [HttpGet]
        public async Task<ApiResultDto> GetSegmentsList()
        {
            //var user = new { UserName = "john", Id = 19 };
            //var serializerSettings = new JsonSerializerSettings
            //{
            //    // 设置为驼峰命名
            //    ContractResolver = new CamelCasePropertyNamesContractResolver()
            //};
            //var userStr = JsonConvert.SerializeObject(user, serializerSettings); 
            //var data = JsonConvert.DeserializeObject<dynamic>(userStr)!;
            //Console.WriteLine(data.UserName + "   " + data.Id);

            var list = _iSegmentsService.GetSegmentsList();
            var segdevList = _iSegmentDeviceService.GetAllList();
            //foreach (var item in list)
            //{
            //    if (item.JsonContent != "")
            //    {
            //var myObj = JsonConvert.DeserializeObject<dynamic>(item.JsonContent!);//反序列化为 dynamic 对象 
            //var setting = new JsonSerializerSettings();
            //setting.ContractResolver = new CamelCasePropertyNamesContractResolver();  //使用驼峰样式  
            //var serializerSettings1 = new JsonSerializerSettings
            //{
            //    // 设置为驼峰命名
            //    ContractResolver = new CamelCasePropertyNamesContractResolver()
            //}; 
            //string jsonString = JsonConvert.SerializeObject(myObj, serializerSettings);
            //myObj = JsonConvert.DeserializeObject<dynamic>(jsonString);
            //jsonString = JsonConvert.SerializeObject(myObj, serializerSettings);
            //Console.WriteLine(jsonString);
            //    }
            //}
            var obj = new
            {
                list,
                segdevList
            };
            return await ApiResult.Success(obj);
        }

        [HttpGet]
        public async Task<ApiResultDto> ValidateSegmentsName(string name, string pid = "0")
        {
            var pFlag = _iSegmentsService.ValidateName(name, int.Parse(pid));
            return await ApiResult.Success(pFlag);
        }

        [HttpPost]
        public async Task<ApiResultDto> SaveSegments(SegmentsDto req)
        {
            _iSegmentsService.SaveSegments(req);
            return await ApiResult.Success(true);
        }

        [HttpGet]
        public async Task<ApiResultDto> UpdateSegmentsName(string name, string id)
        {
            var pFlag = _iSegmentsService.UpdateName(name, int.Parse(id));
            return await ApiResult.Success(pFlag);
        }
        [HttpGet]
        public async Task<ApiResultDto> UpdateSegmentsStatus(string id)
        {
            var pFlag = _iSegmentsService.UpdateStatus(int.Parse(id));
            return await ApiResult.Success(pFlag);
        }
        #endregion 

        #region 网络段支持的终端设备
        [HttpGet]
        public async Task<ApiResultDto> GetSegmentDeviceList(string id)
        {
            var list = _iSegmentDeviceService.GetList(int.Parse(id));
            return await ApiResult.Success(list);
        } 

        [HttpPost]
        public async Task<ApiResultDto> SaveSegmentDevice(List<SegmentDeviceDto> req)
        {
            _iSegmentDeviceService.Saves(req);
            return await ApiResult.Success(true);
        }

        [HttpGet]
        public async Task<ApiResultDto> UpdateSegmentDeviceStatus(string id)
        {
            var list = _iSegmentDeviceService.UpdateStatus(int.Parse(id));
            return await ApiResult.Success(list);
        }
        #endregion

        #region 控制器
        [HttpGet]
        public async Task<ApiResultDto> GetControlsList()
        {
            var list = _iControlsService.GetList();
            return await ApiResult.Success(list);
        }

        [HttpGet]
        public async Task<ApiResultDto> ValidateControlsName(string name, string pid = "0")
        {
            var pFlag = _iControlsService.ValidateName(name, int.Parse(pid));
            return await ApiResult.Success(pFlag);
        }

        [HttpPost]
        public async Task<ApiResultDto> SaveControls(ControlsDto req)
        {
            _iControlsService.Saves(req);
            return await ApiResult.Success(true);
        }

        [HttpGet]
        public async Task<ApiResultDto> UpdateControlsName(string name, string id)
        {
            var pFlag = _iControlsService.UpdateName(name, int.Parse(id));
            return await ApiResult.Success(pFlag);
        }
        [HttpGet]
        public async Task<ApiResultDto> UpdateControlsStatus(string id)
        {
            var pFlag = _iControlsService.UpdateStatus(int.Parse(id));
            return await ApiResult.Success(pFlag);
        }
        #endregion

        #region 资源
        [HttpGet]
        public async Task<ApiResultDto> GetResourceList()
        {
            var list = _iResourcesService.GetList();
            return await ApiResult.Success(list);
        }

   
        [HttpPost]
        public async Task<ApiResultDto> SaveResource(ResourcesDto req)
        {
            _iResourcesService.Saves(req);
            return await ApiResult.Success(true);
        }

        [HttpGet]
        public async Task<ApiResultDto> UpdateResourcesStatus(string id)
        {
            var pFlag = _iResourcesService.UpdateStatus(int.Parse(id));
            return await ApiResult.Success(pFlag);
        }
        #endregion

        #region 资源功能块
        [HttpGet]
        public async Task<ApiResultDto> GetResourceFuncList()
        {
            var list = _iResourceFuncsService.GetList();
            return await ApiResult.Success(list);
        }
        [HttpGet]
        public async Task<ApiResultDto> GetResourceFuncByTypeGroup()
        {
            var list = _iResourceFuncsService.GetList();
            var listGroup=list.GroupBy(x => x.Type).Select(y => new { type=y.Key, list=y }).ToList();
            return await ApiResult.Success(listGroup);
        }

        [HttpPost]
        public async Task<ApiResultDto> SaveResourceFunc(ResourceFuncsDto req)
        {
            _iResourceFuncsService.Saves(req);
            return await ApiResult.Success(true);
        }

        [HttpGet]
        public async Task<ApiResultDto> UpdateResourceFuncStatus(string id)
        {
            var pFlag = _iResourceFuncsService.UpdateStatus(int.Parse(id));
            return await ApiResult.Success(pFlag);
        }
        #endregion

        #region 通用功能块-内置功能块


        [HttpGet]  
        public async Task<ApiResultDto> GetInternalFbsList()
        {
            var list = _iInternalFbsService.GetList();

            List<InternalFbsDto> lsIfbs= list.Where(x=>!string.IsNullOrWhiteSpace(x.JsonContent)).ToList();
            foreach (var item in lsIfbs)
            {
                item.Type = "SFB";
                item.ParentName = list.Find(x => x.Id == item.ParentId)!?.Name;
            }

            //foreach (var item in list)
            //{
            //if (!string.IsNullOrEmpty(item.XmlContent) )
            // {
            //     string xmlContent = item.XmlContent!.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>","").Trim();
            //     xmlContent = Regex.Replace(xmlContent, "<VarDeclaration", "<VarDeclaration xmlns:json=\"http://james.newtonking.com/projects/json\" json:Array=\"true\" ");
            //     XmlDocument xmlDoc = new XmlDocument();
            //     xmlDoc.LoadXml(xmlContent);
            //     string jsonText = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.None, true);
            //     Regex reg = new Regex("\"@([^ \"]*)\"\\s*:\\s*\"(([^ \"]+\\s*)*)\"");
            //     string strPatten = "\"item\":\"2\"";
            //     jsonText = reg.Replace(jsonText, strPatten);
            //     item.JsonContent = jsonText;
            //     _iInternalFbsService.Saves(item);
            // }
            //}
            return await ApiResult.Success(new { list, lsIfbs });
        }


        [HttpGet]
        public async Task<ApiResultDto> ValidateInternalFbsName(string name, string pid = "0")
        {
            var pFlag = _iInternalFbsService.ValidateName(name, int.Parse(pid));
            return await ApiResult.Success(pFlag);
        }

        [HttpPost]
        public async Task<ApiResultDto> SaveInternalFbs(InternalFbsDto req)
        {
            _iInternalFbsService.Saves(req);
            return await ApiResult.Success(true);
        }

        [HttpGet]
        public async Task<ApiResultDto> UpdateInternalFbsName(string name, string id)
        {
            var pFlag = _iInternalFbsService.UpdateName(name, int.Parse(id));
            return await ApiResult.Success(pFlag);
        }
        [HttpGet]
        public async Task<ApiResultDto> UpdateInternalFbsStatus(string id)
        {
            var pFlag = _iInternalFbsService.UpdateStatus(int.Parse(id));
            return await ApiResult.Success(pFlag);
        }
        #endregion

        #region 上传文件
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto> UploadFileAsync([FromForm] IFormFile file , [FromForm] string types)
        {
            //IFormFile file = files[0];
            string suffix = string.Empty; //取出后缀名
            #region 获取文件后缀 
            string filename = file.FileName.Trim();
            int index = filename.LastIndexOf(".");
            if (index > -1)
            {
                suffix = filename.Substring(index);
            }
            string name = filename.Substring(0, index);
            string[] arrayType = types.Split(',', StringSplitOptions.None);
            string type = arrayType[0];
            int parentId = int.Parse(arrayType[1]);
            string rallyExtension = arrayType[2];

            #endregion
            if (arrayType.Length == 3)
            {
                if (rallyExtension == suffix)
                {
                    switch (type)
                    {
                        case "seg":
                            return await UploadExportSegments(file, name, parentId);
                        case "dev":
                            return await UploadExportDevices(file, name, parentId);
                        case "control":
                            return await UploadExportControls(file, name, parentId);
                        case "resource":
                            return await UploadExportResource(file, name, parentId);
                        case "resourcefunc":
                            return await UploadExportResourceFunc(file, name, parentId);
                        case "fbt":
                            return await UploadExportInternalFbs(file, name, parentId);
                        default:
                            return await ApiResult.Success(false);
                    }
                }
                else
                { 
                    return await ApiResult.Error("上传数据数据格式不正确");
                }
            }
            else if (arrayType.Length == 4) //上传图片
            {
                if (".jpg,.jpge,.png,.bmp".IndexOf(suffix.ToLower())>=0)
                {
                    return await UploadImageAsync(file,type, parentId);
                    //switch (type)
                    //{
                    //    case "seg":
                    //        return await UploadExportSegments(file, name, parentId);
                    //    case "dev":
                    //        return await UploadExportDevices(file, name, parentId);
                    //    case "control":
                    //        return await UploadExportControls(file, name, parentId);
                    //    case "resource":
                    //        return await UploadExportResource(file, name, parentId);
                    //    case "resourcefunc":
                    //        return await UploadExportResourceFunc(file, name, parentId);
                    //    default:
                    //        return await ApiResult.Success(false);
                    //}
                }
                else
                {
                    return await ApiResult.Error("上传数据数据格式不正确");
                }
            }
            else
            {
                return await ApiResult.Error("上传数据失败");
            }
        }


        private (string,string) XmlConvertJson(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            string xmlContent = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
            //xmlContent = Regex.Replace(xmlContent, "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n", "");
            var regex = new Regex(@"(<\?xml).*?(>)");
            xmlContent = regex.Replace(xmlContent, "");
            xmlContent = Regex.Replace(xmlContent, "<VarDeclaration", "<VarDeclaration xmlns:json=\"http://james.newtonking.com/projects/json\" json:Array=\"true\" ");
            xmlContent = Regex.Replace(xmlContent, "<Event Key=", "<Event xmlns:json=\"http://james.newtonking.com/projects/json\" json:Array=\"true\"  Key=");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);
            string jsonText = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.None, true);
            Regex reg = new Regex("\"@([^ \"]*)\"\\s*:\\s*\"(([^ \"]+\\s*)*)\"");
            string strPatten = "\"$1\":\"$2\"";
            jsonText = reg.Replace(jsonText, strPatten);
            return (xmlContent,jsonText);
        }

        private async Task<ApiResultDto> UploadExportSegments(IFormFile file, string name, int parentId)
        {
            var (xmlContent, jsonText) = XmlConvertJson(file);
            var dyObj = JsonConvert.DeserializeObject<dynamic>(jsonText);
            string type = dyObj!.Type;
            if (type == null
                || type != "")
            {
                return await ApiResult.Error("上传数据内容不正确");
            }
            else
            {
                string images = dyObj!.Color;
                string version = dyObj!.VersionInfo.Version;
                string jsonName = dyObj!.Name;
                var objDto = _iSegmentsService.GetObj(jsonName, parentId);
                if (objDto == null)
                {
                    objDto = _iSegmentsService.GetObj(jsonName, version, parentId);
                    var id = 0;
                    if (objDto != null) { id = objDto.Id; }
                    SegmentsDto req = new SegmentsDto()
                    {
                        Id = id,
                        ParentId = parentId,
                        JsonContent = jsonText,
                        Name = name,
                        XmlContent = xmlContent,
                        Status = 1,
                        Images = images,
                        Version = version,
                    };
                    _iSegmentsService.SaveSegments(req);
                    return await ApiResult.Success("上传成功");
                }
                else
                {
                    return await ApiResult.Error("该网络段已经上传至其他分组");
                }
            }
        }

        private async Task<ApiResultDto> UploadExportDevices(IFormFile file, string name, int parentId)
        {
            var (xmlContent, jsonText) = XmlConvertJson(file);
            var dyObj = JsonConvert.DeserializeObject<dynamic>(jsonText);
            string type = dyObj!.Type;
            if (type==null
                || type != "target_device")
            {
                return await ApiResult.Error("上传数据内容不正确");
            }
            else
            {
                string images = dyObj!.Icon;
                string version = dyObj!.VersionInfo.Version;
                string jsonName = dyObj!.Name;
                var objDto = _iDevicesService.GetObj(jsonName, parentId);
                if (objDto == null)
                {
                    objDto = _iDevicesService.GetObj(jsonName, version, parentId);
                    var id = 0;
                    if (objDto != null) { id = objDto.Id; }
                    DevicesDto req = new DevicesDto()
                    {
                        Id = id,
                        ParentId = parentId,
                        JsonContent = jsonText,
                        Name = jsonName,
                        XmlContent = xmlContent,
                        Status = 1, 
                        Images = images,
                        Version = version
                    };
                    _iDevicesService.SaveDevices(req);
                    return await ApiResult.Success("上传成功");
                }
                else
                {
                    return await ApiResult.Error("该终端设备已经上传至其他分组");
                }
            }

        }

        private async Task<ApiResultDto> UploadExportControls(IFormFile file, string name, int parentId)
        {
            var (xmlContent, jsonText) = XmlConvertJson(file);
            var dyObj = JsonConvert.DeserializeObject<dynamic>(jsonText);
            string type = dyObj!.Type;
            if (type == null || type != "device")
            {
                return await ApiResult.Error("上传数据内容不正确");
            }
            else
            {
                string images = dyObj!.Icon;
                string version = dyObj!.VersionInfo.Version;
                string jsonName = dyObj!.Name; 
                var objDto = _iControlsService.GetObj(jsonName, parentId);
                if (objDto == null)
                {
                    objDto = _iControlsService.GetObj(jsonName,version, parentId);
                    var id = 0;
                    if(objDto!=null) { id= objDto.Id; }
                    ControlsDto req = new ControlsDto()
                    {
                        Id = id,
                        ParentId = parentId,
                        JsonContent = jsonText,
                        Name = jsonName,
                        XmlContent = xmlContent,
                        Status = 1,
                        Images = images,
                        Version = version,
                    };
                    _iControlsService.Saves(req);
                    return await ApiResult.Success("上传成功");
                }
                else
                {
                    return await ApiResult.Error("该控制器已经上传至其他分组");
                }
            }
        }

        private async Task<ApiResultDto> UploadExportResource(IFormFile file, string name, int parentId)
        {
            var (xmlContent, jsonText) = XmlConvertJson(file);
            var dyObj = JsonConvert.DeserializeObject<dynamic>(jsonText);
            string type = dyObj!.Type;
            if (type == null || type != "")
            {
                return await ApiResult.Error("上传数据内容不正确");
            }
            else
            { 
                string version = dyObj!.VersionInfo.Version;
                string jsonName = dyObj!.Name;
                var objDto = _iResourcesService.GetObj(jsonName, version, parentId);
                var id = 0;
                if (objDto != null) { id = objDto.Id; }
                ResourcesDto req = new ResourcesDto()
                {
                    Id = id,
                    ParentId = parentId,
                    JsonContent = jsonText,
                    Name = name,
                    XmlContent = xmlContent,
                    Status = 1,
                };
                _iResourcesService.Saves(req);
                return await ApiResult.Success("上传成功");
            }

        }

        private async Task<ApiResultDto> UploadExportResourceFunc(IFormFile file, string name, int parentId)
        {
            var (xmlContent, jsonText) = XmlConvertJson(file);
            var dyObj= JsonConvert.DeserializeObject<dynamic>(jsonText);
            string type = dyObj!.Type;
            string version = dyObj!.Version;
            string reName=dyObj!.Name;
            ResourceFuncsDto req = new ResourceFuncsDto()
            {
                Id = 0,
                ParentId = parentId,
                JsonContent = jsonText,
                Name = reName,
                Namespace=name,
                XmlContent = xmlContent,
                Status = 1,
                Type = type,
                Version = version,
            };
            _iResourceFuncsService.Saves(req);
            return await ApiResult.Success("上传成功"); 
        }

        private async Task<ApiResultDto> UploadExportInternalFbs(IFormFile file, string name, int parentId)
        {
            var (xmlContent, jsonText) = XmlConvertJson(file);
            var dyObj = JsonConvert.DeserializeObject<dynamic>(jsonText);
            string type = dyObj!.Type;
            if (!string.IsNullOrWhiteSpace(""))
            {
                return await ApiResult.Error("上传数据内容不正确");
            }
            else
            {
                string version = dyObj!.Version;
                string jsonName = dyObj!.Name;
                var objDto = _iInternalFbsService.GetObj(jsonName, parentId);
                if (objDto == null)
                {
                     objDto = _iInternalFbsService.GetObj(jsonName, version, parentId);
                    var id = 0;
                    if (objDto != null) { id = objDto.Id; }
                    InternalFbsDto req = new InternalFbsDto()
                    {
                        Id = id,
                        ParentId = parentId,
                        JsonContent = jsonText,
                        Name = name,
                        XmlContent = xmlContent,
                        Status = 1,
                        Version = version,
                    };
                    _iInternalFbsService.Saves(req);
                    return await ApiResult.Success("上传成功");
                }
                else
                {
                    return await ApiResult.Error("该功能块已经上传至其他分组");
                }
            }

        }


        //private bool CheckExtensionName(string extensionName)
        //{
        //    bool pFlag = false;
        //    switch (extensionName.ToLower())
        //    {
        //        case ".dev":
        //        case ".seg":
        //        case ".res":
        //        case ".fbt":
        //            pFlag = true; break;
        //        default:
        //            pFlag = false; break;
        //    }
        //    return pFlag;
        //}


        private async Task<ApiResultDto> UploadImageAsync(IFormFile file,string type,int id)
        {
            string appDirectory = AppContext.BaseDirectory;
            string ImageFolder = _iConfig["ImageFolder"]!;
            string saveDirectory = Path.Combine(appDirectory, ImageFolder);
            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            } 
            string sourceName = file.FileName.Trim();
            //组合一个完整的路径--包含文件件+文件名 
            string newFilePath = $"{saveDirectory}{sourceName}";
            try
            {
                using (FileStream stream = System.IO.File.Create(newFilePath))
                {
                    await file.CopyToAsync(stream);

                    switch (type)
                    {
                        //case "seg":
                        //    _iSegmentsService.UpdateImage(sourceName, id); break;
                        case "dev":
                            _iDevicesService.UpdateImage(sourceName, id); break;
                        case "control":
                            _iControlsService.UpdateImage(sourceName, id); break;
                        //case "resource":
                        //    return   UploadExportResource(file, name, parentId);
                        //case "resourcefunc":
                        //    return await UploadExportResourceFunc(file, name, parentId);
                        default:break; 
                    }
                    var fileContent = new
                    {
                        SourceFileName = sourceName,
                        LinkFilePath = $"{ImageFolder}{sourceName}",
                        //FullFilePath = newFilePath
                    };
                    return await ApiResult.Success(fileContent,"图片上传成功");
                }
            }
            catch (Exception ex)
            {
                return await ApiResult.Error("文件上传失败");
            }

        }
         
        #endregion
    }

}
