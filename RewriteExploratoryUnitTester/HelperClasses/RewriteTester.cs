using System;
using System.Collections.Generic;
using System.Linq;
using RewriteExploratoryUnitTester.Containers;
using RewriteExploratoryUnitTester.DataSource;
using RewriteExploratoryUnitTester.Processors;

namespace RewriteExploratoryUnitTester.HelperClasses
{
    public class RewriteTester
    {
        readonly RewriteFactory _factory;
        public List<RewriteRuleSet> RulesSets { get; private set; }

        public RewriteTester(RewriteFactory factory)
        {
            _factory = factory;
        }

        public void LoadConfig(string path)
        {
            var rr = new RewriteReader(_factory);
            var conf = rr.ReadConf(path);

            RulesSets = RewriteRuleSet.BuildRuleSets(conf).ToList();
        }

        public RedirectData TestUrl(string url, bool outputAllMatching = false)
        {
            if (RulesSets == null) throw new Exception("Did you forget to call LoadConfig?");


            var data = new RedirectData(url);
            var matchesRuleSets = RulesSets.Where(r => r.ProcessConditions(ref data));

            foreach (var rule in matchesRuleSets)
            {
                data = rule.ProcessRules(data);
                if (!outputAllMatching && data.Status == RedirectStatus.Redirected)
                    break;
            }
            return data;
        }
    }
}
