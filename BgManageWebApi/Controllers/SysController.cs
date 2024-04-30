using BusinessEntity.Other;
using BusinessEntity;
using BusinessInterface;
using BusinessService;
using CommonUtility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessEntity.Sys;

namespace BgManageWebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class SysController : ControllerBase
    {
        private readonly ISysFuncService _iSysFuncService;
        public SysController(ISysFuncService iSysFuncService)
        {
            _iSysFuncService = iSysFuncService;
        }

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
    }
}
