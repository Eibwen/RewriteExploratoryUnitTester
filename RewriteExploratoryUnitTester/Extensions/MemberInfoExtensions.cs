using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RewriteExploratoryUnitTester.Extensions
{
    public static class MemberInfoExtensions
    {
        public static IEnumerable<KeyValuePair<string, string>> OptionField(this MemberInfo method)
        {
            var attribs = method.GetCustomAttributes(typeof(TextAttribute), false)
                                .Cast<TextAttribute>().ToArray();

            if (attribs.Length == 0) yield break;

            foreach (var a in attribs)
                yield return new KeyValuePair<string, string>(a.Value, method.Name);
        }
    }
}
