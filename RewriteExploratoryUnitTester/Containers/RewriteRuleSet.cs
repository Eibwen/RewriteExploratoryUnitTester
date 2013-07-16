using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RewriteExploratoryUnitTester.Containers
{
    public class RewriteRuleSet
    {
        ///Probably move this out...
        public static IEnumerable<RewriteRuleSet> BuildRuleSets(IEnumerable<IRedirectLine> conf)
        {
            RewriteRuleSet rules = new RewriteRuleSet();
            foreach (var c in conf)
            {
                if (c == null)
                {
                    //Ignored line... might be alright
                    continue;
                }

                if (c.IsCondition)
                {
                    if (rules.Rules != null)
                    {
                        //Have condition and rules, move onto a new ruleset
                        yield return rules;
                        rules = new RewriteRuleSet();
                    }
                    if (rules.Conditions == null) rules.Conditions = new List<RewriteCondition>();

                    rules.Conditions.Add((RewriteCondition)c);
                }
                else if (c.IsRule)
                {
                    if (rules.Conditions == null)
                    {
                        throw new Exception("Rule without conditions... wtf, this isn't allowed!...?");
                    }
                    if (rules.Rules == null) rules.Rules = new List<RewriteRule>();

                    rules.Rules.Add((RewriteRule)c);
                }
            }

            if (rules.Rules == null) throw new Exception("Did this file end with a condition, wtf?");

            yield return rules;
        }

        public List<RewriteCondition> Conditions { get; set; }
        public List<RewriteRule> Rules { get; set; }

        public bool ProcessConditions(string url)
        {
            throw new NotImplementedException();
            //return Conditions.All(c => c.Process(url));
        }
        public RedirectData ProcessRules(string url)
        {
            throw new NotImplementedException();
        }
    }
}
