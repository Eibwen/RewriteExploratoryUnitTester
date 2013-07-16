using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RewriteExploratoryUnitTester.Extensions;

namespace RewriteExploratoryUnitTester.DataSource
{
    [Flags]
    public enum RuleOptions
    {
        None = 0x00000000,
        [Text("NC")] //ISAPI
        [Text("I")] //Apache
        NoCase = 0x00000001,
        [Text("R=301")]
        Redirect301 = 0x00000010,
        [Text("F")]
        Forbidden = 0x00000100,
        [Text("L")]
        LastRule = 0x00001000,
        [Text("O")]
        Normalize = 0x00010000,
        [Text("NE")]
        ///By default, special characters, such as & and ?, for example, will be converted to their hexcode equivalent. Using the [NE] flag prevents that from happening.
        NoEscape = 0x00100000,
        [Text("NU")]
        NoUnicode = 0x01000000,
        [Text("U")]
        ///Log the URL as it was originally requested and not as the URL was rewritten.
        UnmangleLog = 0x01000000,
        [Text("CL")]
        //Changes the case of substitution result to lower.
        CaseLower = 0x10000000,


        FINISHED = LastRule | Redirect301
    }
}
