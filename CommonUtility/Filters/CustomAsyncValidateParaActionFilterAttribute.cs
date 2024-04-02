using BusinessEntity;
using BusinessEntity.Base;
using BusinessEntity.Other;
using BusinessEntity.Other.ValidateRules;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace CommonUtility.Filters
{

    /// <summary>
    /// 参数校验
    /// </summary>
    public class CustomAsyncValidateParaActionFilterAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //这里是验证的业务逻辑--验证参数
            List<object?> parameterList = context.ActionArguments
                .Where(p => p.Value is BaseDto && p.Value is not null)
                .Select(c => c.Value)
                .ToList();

            List<(bool, string?)> messaglist = new List<(bool, string?)>();
            foreach (var parameter in parameterList)
            {
                //实体验证的实现--通过特性
                foreach (var prop in parameter!.GetType().GetProperties().Where(p => p.IsDefined(typeof(BaseAbstractAttribute), true)))
                {
                    BaseAbstractAttribute attribute = prop.GetCustomAttribute<BaseAbstractAttribute>()!;
                    messaglist.Add(attribute.DoValidate(prop.GetValue(parameter)!));
                }
            }

            if (messaglist.Any(c => c.Item1 == false))
            {
                context.Result = new JsonResult(new ApiResultDto()
                {
                    Data = messaglist.Where(c => c.Item1 == false).Select(c => c.Item2),
                    IsSuccess = false,
                    Message = ""
                }); ;
            }
            else
            {
                await next();
            } 
        }
    }
}
