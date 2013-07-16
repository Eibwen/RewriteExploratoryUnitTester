using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RewriteExploratoryUnitTester.DataSource;

namespace RewriteExploratoryUnitTester.Containers
{
    public class RedirectData
    {
        public string OriginalUrl { get; set; }

        public string ProcessedUrl { get; set; }
        public RedirectStatus Status { get; set; }

        //TODO this goes in a RuleSetMatchData ?
        public List<string> ConditionMatchGroups { get; set; }
        public List<string> RuleMatchGroups { get; set; }
    }
}
