using System;
using System.Collections.Generic;
using RewriteExploratoryUnitTester.DataSource;

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
                if (c == null || c.LineType == RedirectLineType.Ignore)
                {
                    //Ignored line... might be alright
                    continue;
                }

                if (c.LineType == RedirectLineType.Condition)
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
                else if (c.LineType == RedirectLineType.Rule)
                {
                    //Actually only one rule* per Condition
                    //TODO or other types: RewriteRule, RewriteHeader or RewriteProxy
                    if (rules.Rules != null)
                    {
                        //Have condition and rules, move onto a new ruleset
                        yield return rules;
                        rules = new RewriteRuleSet();
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

        public bool ProcessConditions(ref RedirectData data)
        {
            //Default to true
            if (Conditions == null) return true;

            foreach (var c in Conditions)
            {
                try
                {
                    if (!c.MatchesCondition(ref data)) return false;
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed processing condition from line: " + c.LineNumber, ex);
                }
            }
            //data.RuleSetMatched = this;
            return true;
        }

        public RedirectData ProcessRules(RedirectData data)
        {
            foreach (var rule in Rules)
            {
                try
                {
                    data = rule.ProcessRule(data);
                    switch (data.Status)
                    {
                        case RedirectStatus.NotProcessed:
                        //case RedirectStatus.Continue:
                            continue;
                        case RedirectStatus.Modified:
                            data.RuleSetMatched = this;
                            continue;
                        case RedirectStatus.Redirected:
                            data.RuleSetMatched = this;
                            break;
                        default:
                            throw new Exception("Unknown RedirectStatus... code needs to be updated: " + data.Status.ToString());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed processing rule from line: " + rule.LineNumber, ex);
                }
            }
            return data;
        }
    }
}
