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

        public RedirectData ProcessRule(RedirectData data)
        {
            var m = Regex.Match(data.OriginalUrl, MatchPattern);
            if (m.Success)
            {
                data.RuleMatchGroups = m.Groups.Cast<Match>()
                                        .Select(a => a.Value).ToList();

                data.ProcessedUrl = BuildOutputUrl(data);

                if ((Options & RuleOptions.FINISHED) > 0)
                {
                    data.Status = RedirectStatus.Redirected;
                }
            }
            return data;
        }

        public string BuildOutputUrl(RedirectData data)
        {
            var ruleMatches = data.RuleMatchGroups ?? new List<string>();
            var conditionMatches = data.ConditionMatchGroups ?? new List<string>();

            return Regex.Replace(ReplacePattern, @"($\d|%\d|$$\d\d|%%\d\d)", match => Evaluator(match, ruleMatches, conditionMatches));
        }

        private string Evaluator(Match match, List<string> ruleMatches, List<string> conditionMatches)
        {
            var m = match.Value;
            if (m[0] == '$')
            {
                m = m.TrimStart('$');
                var i = Int32.Parse(m);

                if (i < ruleMatches.Count) return ruleMatches[i];
                return "";
            }
            else if (m[0] == '%')
            {
                m = m.TrimStart('%');
                var i = Int32.Parse(m);

                if (i < conditionMatches.Count) return conditionMatches[i];
                return "";
            }
            return m;
        }


        public RedirectLineType LineType
        {
            get { return RedirectLineType.Rule; }
        }
    }
}
