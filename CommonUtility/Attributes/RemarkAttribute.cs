using System;
using System.Collections.Generic;
using System.Text;

namespace CommonUtility.Attributes
{
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
    public class RemarkAttribute:System.Attribute 
    {
        public string Remark  { get; set; }
        public RemarkAttribute(string _remark)
        {
            this.Remark = _remark;
        }

        public string GetRemark()
        {
            return this.Remark;
        }
    }
}
