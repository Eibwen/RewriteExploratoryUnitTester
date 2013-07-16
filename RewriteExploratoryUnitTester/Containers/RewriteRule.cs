using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RewriteExploratoryUnitTester.DataSource;

namespace RewriteExploratoryUnitTester.Containers
{
    public class RewriteRule : IRedirectLine
    {
        static readonly Regex RewriteRulePattern = new Regex(@"^[Rr]ewriteRule (?<match>[^ ]+) +(?<replace>[^ ]+)(?: +\[(?<options>[^\]]+)\])?$");
        public RewriteRule(string line)
        {
            var m = RewriteRulePattern.Match(line);

            if (!m.Success) throw new Exception("FAIL: '" + line + "'");

            //if (lines[0] == "RewriteRule")
            MatchPattern = m.Groups["match"].Value;
            ReplacePattern = m.Groups["replace"].Value;
            if (m.Groups["options"].Success)
                Options = OptionsFactory.BuildOptions(m.Groups["options"].Value);
        }

        public string MatchPattern { get; set; }
        public string ReplacePattern { get; set; }
        public RuleOptions Options { get; set; }


        public bool WillProcess(RedirectData data)
        {
            return true;
        }
        public RedirectData Process(RedirectData data)
        {
            throw new NotImplementedException();
        }
        public bool IsCondition { get { return false; } }
        public bool IsRule { get { return true; } }
    }
}
