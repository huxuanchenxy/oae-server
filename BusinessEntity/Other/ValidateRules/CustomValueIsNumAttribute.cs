namespace BusinessEntity.Other.ValidateRules
{

    /// <summary>
    /// 验证是否为数字
    /// </summary>
    public class CustomValueIsNumAttribute : BaseAbstractAttribute
    { 
        public CustomValueIsNumAttribute(string? messge) : base(messge)
        {
        }

        public override (bool, string?) DoValidate(object oValue)
        {
            return (true, "");
        }
    }
}
