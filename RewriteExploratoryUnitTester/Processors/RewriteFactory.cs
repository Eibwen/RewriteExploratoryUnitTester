using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RewriteExploratoryUnitTester.Containers;
using RewriteExploratoryUnitTester.DataSource;

namespace RewriteExploratoryUnitTester.Processors
{
    public class RewriteFactory
    {
        readonly ISampleValues _values;
        public RewriteFactory(ISampleValues values)
        {
            _values = values;
        }

        List<string> IgnoredSettings = new List<string>
	{
		"RewriteEngine",
		"RewriteCompatibility2",
		"RepeatLimit",
		"RewriteBase"
	};

        public IRedirectLine Build(int lineNumber, string line)
        {
            if (line.StartsWith("RewriteRule")
                //Someone missed a shift key... only one person, one instance
                || line.StartsWith("rewriteRule"))
            {
                return new RewriteRule(lineNumber, line);
            }
            else if (line.StartsWith("RewriteCond"))
            {
                return new RewriteCondition(lineNumber, line, _values);
            }
            else if (IgnoredSettings.Any(x => line.StartsWith(x)))
            {
                //Ignored
                return null;
            }
            //line.Dump();
            throw new Exception("Unknown line: " + line);
        }
        //	public RewriteCondition Build(string line, RewriteCondition lastCondition)
        //	{
        //		if (line.StartsWith("RewriteRule")
        //			//Someone missed a shift key... only one person, one instance
        //			|| line.StartsWith("rewriteRule"))
        //		{
        //			if (lastCondition == null) throw new Exception("Missing a first condition??? is that allowed?");
        //			
        //			lastCondition.Rules.Add(new RewriteRule(line));
        //			return lastCondition;
        //		}
        //		else if (line.StartsWith("RewriteCond"))
        //		{
        //			return new RewriteCondition(line);
        //		}
        //		else if (IgnoredSettings.Any(x => line.StartsWith(x)))
        //		{
        //			//Ignored
        //			return null;
        //		}
        //		else
        //		{
        //			line.Dump();
        //		}
        //		throw new Exception("Unknown line: " + line);
        //	}
    }
}
