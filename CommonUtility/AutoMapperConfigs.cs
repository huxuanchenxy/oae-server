using AutoMapper;
using BusinessEntity;
using DataBaseModels.Common;
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
            CreateMap<TestEntity, Test>().ReverseMap();
        }
    }
}
