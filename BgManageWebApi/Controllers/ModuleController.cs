using BusinessEntity;
using BusinessEntity.Other;
using BusinessInterface;
using CommonUtility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BgManageWebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        ICusModuleService _iCusModuleService;
        public ModuleController(ICusModuleService iCusModuleService)
        {
            _iCusModuleService = iCusModuleService;
        }


        /// <summary>
        /// 获取模块信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResultDto> GetModule(string id)
        { 
            var obj = _iCusModuleService.GetModule(int.Parse(id));
            return await ApiResult.Success(obj);
        } 

        [HttpPost]
        public async Task<ApiResultDto> SaveModule(List<CusModuleDto> dtoList)
        {
            var iCount = _iCusModuleService.SaveModule(dtoList);
            return await ApiResult.Success(iCount);
        }

       



    }
}