using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Other.ValidateRules
{
    public abstract class BaseAbstractAttribute : Attribute
    {
        public BaseAbstractAttribute(string? messge)
        {
            Message = messge;
        }

        protected string? Message { get; set; }

        public abstract (bool, string?) DoValidate(object oValue);
    }
}
