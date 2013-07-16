using System;

namespace RewriteExploratoryUnitTester.Extensions
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class TextAttribute : Attribute
    {
        public TextAttribute(string value)
        {
            Value = value;
        }
        public string Value { get; set; }
    }
}
