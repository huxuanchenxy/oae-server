using AutoMapper;
using BusinessEntity;
using BusinessEntity.Sys;
using DataBaseModels;
using DataBaseModels.Common;
using DataBaseModels.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CommonUtility
{
    public class AutoMapperConfigs : Profile
    {
        /// <summary>
        /// 配置映射关系  
        /// </summary>
        public AutoMapperConfigs()
        {
            CreateMap<TestDto, Test>().ReverseMap();
            CreateMap<SysFuncDto, SysFunc>().ReverseMap();
            CreateMap<CusModuleDto, CusModule>().ReverseMap();
            CreateMap<FunctionsDto, Functions>().ReverseMap(); 
            CreateMap<SegmentsDto, Segments>().ReverseMap();
            CreateMap<DevicesDto, Devices>().ReverseMap();
            CreateMap<ControlsDto, Controls>().ReverseMap();
            CreateMap<ResourcesDto, Resources>().ReverseMap();
            CreateMap<ResourcesDto, TreeListDto>().ReverseMap();
            CreateMap<SegmentsDto, TreeListDto>().ReverseMap();
            CreateMap<DevicesDto, TreeListDto>().ReverseMap();
            CreateMap<ControlsDto, TreeListDto>().ReverseMap();
            CreateMap<ResourceFuncsDto, ResourceFuncs>().ReverseMap();
            CreateMap<SegmentDeviceDto, SegmentDevice>().ReverseMap();
            CreateMap<InternalFbsDto, InternalFbs>().ReverseMap();
            CreateMap<InternalFbsDto, TreeListDto>().ReverseMap();
        }
    }
}
