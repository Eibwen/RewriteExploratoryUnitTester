using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RewriteExploratoryUnitTester.Containers
{
    public interface IRedirectLine
    {
        bool WillProcess(RedirectData data);
        RedirectData Process(RedirectData data);

        //TODO use enum??
        bool IsCondition { get; }
        bool IsRule { get; }
    }
}
