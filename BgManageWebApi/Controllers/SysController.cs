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
    }
}
