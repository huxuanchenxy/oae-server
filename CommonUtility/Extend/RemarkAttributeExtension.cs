using CommonUtility.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CommonUtility.Extend
{
    public static class RemarkAttributeExtension
    {
        public static string GetRemark(this Enum @enum)
        {
            Type type=@enum.GetType();
            string remark = @enum.ToString(); 
            FieldInfo field = type.GetField(remark)!;
            if(field.IsDefined(typeof(RemarkAttribute), true))
            {
                RemarkAttribute attr = (RemarkAttribute)field.GetCustomAttribute<RemarkAttribute>()!;
                remark = attr.GetRemark();
            }
            return remark;
        }
    }
}
