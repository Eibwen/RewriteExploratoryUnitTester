using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RewriteExploratoryUnitTester.DataSource;

namespace RewriteExploratoryUnitTester.Containers
{
    public class RewriteCondition : IRedirectLine
    {
        static readonly Regex RewriteCondPattern = new Regex(@"^RewriteCond (%\{(?<variable>[^\}]+)\} )?(?<match>[^ ]+)$");
        public RewriteCondition(string line, ISampleValues values)
        {
            _values = values;

            var m = RewriteCondPattern.Match(line);

            if (!m.Success) throw new Exception("FAIL: '" + line + "'");

            if (m.Groups["variable"].Success)
                Variable = m.Groups["variable"].Value;
            MatchPattern = m.Groups["match"].Value;
        }

        readonly ISampleValues _values;

        public string Variable { get; set; }
        public string MatchPattern { get; set; }


        public bool WillProcess(RedirectData data)
        {
            throw new NotImplementedException();
        }
        public RedirectData Process(RedirectData data)
        {
            //		for (int i = 0; i < Rules.Count; ++i)
            //		{
            //			if (data.Status == RedirectStatus.Continue)
            //			{
            //				data = Rules[i].Process(data);
            //			}
            //			if (data.Status == RedirectStatus.Redirected)
            //			{
            //				break;
            //			}
            //		}
            //		return data;
            throw new NotImplementedException();
        }
        public bool IsCondition { get { return true; } }
        public bool IsRule { get { return false; } }
    }
}
