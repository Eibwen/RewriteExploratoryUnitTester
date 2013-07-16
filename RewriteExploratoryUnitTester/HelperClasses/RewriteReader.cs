using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RewriteExploratoryUnitTester.Containers;
using RewriteExploratoryUnitTester.Processors;

namespace RewriteExploratoryUnitTester.HelperClasses
{
    public class RewriteReader
    {
        readonly RewriteFactory _fact;
        public RewriteReader(RewriteFactory fact)
        {
            _fact = fact;
        }

        public IEnumerable<IRedirectLine> ReadConf(string path)
        {
            using (var sw = new StreamReader(path))
            {
                var lineNumber = 0;
                while (!sw.EndOfStream)
                {
                    var line = sw.ReadLine().Trim();
                    ++lineNumber;

                    if (line.Length > 0
                        && !line.StartsWith("#"))
                    {
                        yield return _fact.Build(lineNumber, line);
                    }
                }
                //			RewriteCondition lastCondition = null;
                //			while (!sw.EndOfStream)
                //			{
                //				var line = sw.ReadLine().Trim();
                //				if (line.Length > 0
                //					&& !line.StartsWith("#"))
                //				{
                //					var cond = _fact.Build(line, lastCondition);
                //					if (cond != lastCondition) yield return lastCondition;
                //					lastCondition = cond;
                //				}
                //			}
                //			yield return lastCondition;
            }
        }
    }
}
