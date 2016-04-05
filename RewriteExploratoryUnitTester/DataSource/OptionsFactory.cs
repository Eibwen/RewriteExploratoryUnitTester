using System;
using System.Collections.Generic;
using System.Linq;
using RewriteExploratoryUnitTester.Extensions;

namespace RewriteExploratoryUnitTester.DataSource
{
    public static class OptionsFactory
    {
        static Dictionary<string, RuleOptions> OptionLookup;

        static OptionsFactory()
        {
            OptionLookup = typeof(RuleOptions).GetMembers().SelectMany(e => e.OptionField())
                .Where(e => e.Key != null)
                .ToDictionary(k => k.Key, v => (RuleOptions)Enum.Parse(typeof(RuleOptions), v.Value));
            //OptionLookup.Dump();
        }

        public static RuleOptions BuildOptions(string conf)
        {
            var optionStrings = conf.Trim('[', ']').Split(',').Select(o => o.Trim());

            RuleOptions options = RuleOptions.None;

            foreach (var o in optionStrings)
            {
                if (OptionLookup.ContainsKey(o))
                {
                    options |= OptionLookup[o];
                }
                else
                {
                    //conf.Dump();
                    throw new Exception("UNKNOWN OPTION: " + o);
                }
            }

            return options;
        }
    }
}
