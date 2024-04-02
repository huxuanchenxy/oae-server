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
    public class TestController : ControllerBase
    {
        private readonly ITestService _iTestService;
        public TestController(ITestService iTestService)
        {
            _iTestService = iTestService;
        }
        [HttpGet]
        public async Task<ApiResultDto> TestSave()
        {
            TestEntity testEntity = new TestEntity()
            {
                Name = "Test" + new Random().Next(10086),
            }; 
            _iTestService.Save(testEntity);
            return await ApiResult.Success("请求成功");
        }
    }
}
