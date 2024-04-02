using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Other.ValidateRules
{

    /// <summary>
    /// 验证邮箱 
    /// </summary>
    public class CustomEmailAttribute : BaseAbstractAttribute
    {
        public CustomEmailAttribute(string? messge) : base(messge)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oValue"></param>
        /// <returns></returns>
        public override (bool, string?) DoValidate(object oValue)
        {
            bool bResult = true;
            //这里就是校验邮箱的业务逻辑
            if (!string.IsNullOrWhiteSpace(oValue.ToString()))
            {
                string str = @"^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.(com|cn|net)$";
                bResult = System.Text.RegularExpressions.Regex.IsMatch(oValue.ToString()!, str);
            }
            return bResult ? (true, string.Empty) : (false, Message);
        }
    }
}
