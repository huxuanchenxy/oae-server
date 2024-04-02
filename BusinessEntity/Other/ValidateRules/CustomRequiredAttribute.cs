using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Other.ValidateRules
{
    /// <summary>
    /// 验证非空
    /// </summary>
    public class CustomRequiredAttribute : BaseAbstractAttribute
    {

        public CustomRequiredAttribute(string? messge) : base(messge) { }

        public override (bool, string?) DoValidate(object oValue)
        {
            return string.IsNullOrWhiteSpace(oValue.ToString()) ? (false, Message) : (true, string.Empty);
        }

    }
}
