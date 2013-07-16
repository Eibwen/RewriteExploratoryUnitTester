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
        public RewriteCondition(int lineNumber, string line, ISampleValues values)
        {
            LineNumber = lineNumber;

            _values = values;

            var m = RewriteCondPattern.Match(line);

            if (!m.Success) throw new Exception("FAIL: '" + line + "'");

            if (m.Groups["variable"].Success)
                Variable = m.Groups["variable"].Value;
            MatchPattern = m.Groups["match"].Value;
        }

        readonly ISampleValues _values;

        public int LineNumber { get; set; }

        public string Variable { get; set; }
        public string MatchPattern { get; set; }



        public bool MatchesCondition(ref RedirectData data)
        {
            string testString = data.OriginalUrl.PathAndQuery;
            if (Variable != null) testString = _values.GetSampleValue(Variable, data);

            var m = Regex.Match(testString, MatchPattern);
            if (m.Success)
            {
                data.ConditionMatchGroups = m.Groups.Cast<Group>()
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
