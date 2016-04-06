using System.Collections.Generic;

namespace RewriteExploratoryUnitTester.Containers
{
    public class RedirectDataCollection : RedirectData
    {
        public RedirectDataCollection(string url) : base(url)
        {
            RuleSetMatchedChain = new List<RewriteRuleSet>();
        }

        public void AddRuleSet(RewriteRuleSet rule)
        {
            RuleSetMatchedChain.Add(rule);
        }

        public List<RewriteRuleSet> RuleSetMatchedChain { get; private set; }
    }
}