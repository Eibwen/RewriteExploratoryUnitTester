using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RewriteExploratoryUnitTester.DataSource;
using RewriteExploratoryUnitTester.Extensions;

namespace RewriteExploratoryUnitTester.Containers
{
    public class RewriteRule : IRedirectLine
    {
        static readonly Regex RewriteRulePattern = new Regex(@"^[Rr]ewriteRule (?<match>[^ ]+) +(?<replace>[^ ]+)(?: +\[(?<options>[^\]]+)\])?$");
        public RewriteRule(int lineNumber, string line)
        {
            LineNumber = lineNumber;

            var m = RewriteRulePattern.Match(line);

            if (!m.Success) throw new Exception("FAIL: '" + line + "'");

            //if (lines[0] == "RewriteRule")
            MatchPattern = CleanRegexFordotNet(m.Groups["match"].Value);
            ReplacePattern = m.Groups["replace"].Value;
            if (m.Groups["options"].Success)
                Options = OptionsFactory.BuildOptions(m.Groups["options"].Value);
        }

        public virtual string CleanRegexFordotNet(string regex)
        {
            //.Net doesn't support matching lowercase with \u
            return regex.Replace(@"\u", "");
        }

        public int LineNumber { get; set; }

        public string MatchPattern { get; set; }
        public string ReplacePattern { get; set; }
        public RuleOptions Options { get; set; }

        public RedirectData ProcessRule(RedirectData data)
        {
            //TODO actually this is the OriginalString when condition checks that... or something
            //WTF am i saying above... i think i want ProcessedUrl since that will default back to Original if not changed (i.e. its "CurrentUrl")
            var m = Regex.Match(data.CurrentPathAndQuery, MatchPattern, Options_Regex());
            if (m.Success)
            {
                data.RuleMatchGroups = m.Groups.Cast<Group>()
                                        .Select(a => a.Value).ToList();

                data.ProcessedUrl = BuildOutputUrl(data);

                if ((Options & RuleOptions.FINISHED) > 0
                    && data.OriginalUrl.OriginalString != data.ProcessedUrl)
                {
                    data.Status = RedirectStatus.Redirected;
                    data.SetUrlChanged();
                }
                else if (data.OriginalUrl.OriginalString != data.ProcessedUrl)
                {
                    data.Status = RedirectStatus.Modified;
                    data.SetUrlChanged();
                }
                else
                {
                    //Is this CURRENT status? - yes
                    data.Status = RedirectStatus.NotProcessed;
                }
            }
            else
            {
                //Is this CURRENT status? - yes
                data.Status = RedirectStatus.NotProcessed;
            }
            return data;
        }

        public string BuildOutputUrl(RedirectData data)
        {
            var ruleMatches = data.RuleMatchGroups ?? new List<string>();
            var conditionMatches = data.ConditionMatchGroups ?? new List<string>();

            var processedUrl = Regex.Replace(ReplacePattern, @"(\$\d|%\d|\$\$\d\d|%%\d\d)", match => Evaluator(match, ruleMatches, conditionMatches));
            return Options_PostProcessing(processedUrl);
        }

        private string Options_PostProcessing(string processedUrl)
        {
            var outputString = processedUrl;

            if (Options.HasFlags(RuleOptions.CaseLower))
            {
                outputString = outputString.ToLowerInvariant();
            }

            return Regex.Unescape(outputString);
        }

        private RegexOptions Options_Regex()
        {
            var opts = RegexOptions.None;
            if (Options.HasFlags(RuleOptions.NoCase))
            {
                opts |= RegexOptions.IgnoreCase;
            }
            return opts;
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
