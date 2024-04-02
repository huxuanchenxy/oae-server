using BusinessEntity.Other;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CommonUtility.Filters
{
    /// <summary>
    /// 异常处理
    /// </summary>
    public class CustomAsyncExceptionFilterAttribute : Attribute, IAsyncExceptionFilter
    {
        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.ExceptionHandled == false)
            {
                context.Result = new JsonResult(new ApiResultDto()  { IsSuccess = false, Message = context.Exception.Message });
            }
            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}
