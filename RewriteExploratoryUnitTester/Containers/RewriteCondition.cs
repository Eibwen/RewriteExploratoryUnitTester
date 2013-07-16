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



        public bool MatchesCondition(ref RedirectData data)
        {
            var m = Regex.Match(data.OriginalUrl, MatchPattern);
            if (m.Success)
            {
                data.ConditionMatchGroups = m.Groups.Cast<Match>()
                                             .Select(a => a.Value).ToList();
            }
            return m.Success;
        }

        public RedirectLineType LineType
        {
            get { return RedirectLineType.Condition; }
        }
    }
}
