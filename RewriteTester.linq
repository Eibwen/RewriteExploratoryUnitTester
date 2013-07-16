<Query Kind="Program">
  <Connection>
    <ID>2d718e99-470f-4c0f-b396-74f8d890fa57</ID>
    <Persist>true</Persist>
    <Server>barrett</Server>
    <NoPluralization>true</NoPluralization>
    <Database>uShipDevTrunk</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>

void Main()
{
	string PATH = @"C:\inetpub\wwwroot\uShipTrunk\UShip.Web\httpd.conf";
	
	var rf = new RewriteFactory(new SampleValues());
	
	var tester = new RewriteTester(rf);

	
	tester.TestUrl("Motorcycles.uship.com/state/city/");
}


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
	
	public void TestUrl(string url, bool outputAllMatching = false)
	{
		var matchesRuleSets = RulesSets.Where(r => r.ProcessConditions(url));
		
		foreach (var rule in matchesRuleSets)
		{
			RedirectData data = rule.ProcessRules(url);
			if (!outputAllMatching || data.Status != RedirectStatus.Continue)
				break;
		}
	}
}


public interface ISampleValues
{
//	string HTTP_TRUE_CLIENT_IP { get; }
//	string HTTP_Host { get; }
//	string HTTP_User_Agent { get; }
	string GetSampleValue(string variable);
}
public class SampleValues : ISampleValues
{
//	public string HTTP_TRUE_CLIENT_IP
//	{
//		get { return GetRandom(IpExamples); }
//	}
//	
//	public string HTTP_Host
//	{
//		get { return GetRandom(HostExamples); }
//	}
//	
//	public string HTTP_User_Agent
//	{
//		get { return GetRandom(UserAgentExamples); }
//	}
	
	public string GetSampleValue(string variable)
	{
		switch (variable)
		{
			
		}
		throw new Exception("Unknown variable: " + variable);
	}
	
	readonly Random rand = new Random();
	
	string GetRandom(List<string> source)
	{
		return source[rand.Next(0, source.Count)];
	}
	
	List<string> IpExamples = new List<string>
	{
		"192.168.5.155",
		"166.78.137.146"
	};
	List<string> HostExamples = new List<string>
	{
	};
	List<string> UserAgentExamples = new List<string>
	{
	};
}



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
			while (!sw.EndOfStream)
			{
				var line = sw.ReadLine().Trim();
				if (line.Length > 0
					&& !line.StartsWith("#"))
				{
					yield return _fact.Build(line);
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
	
	public IRedirectLine Build(string line)
	{
		if (line.StartsWith("RewriteRule")
			//Someone missed a shift key... only one person, one instance
			|| line.StartsWith("rewriteRule"))
		{
			return new RewriteRule(line);
		}
		else if (line.StartsWith("RewriteCond"))
		{
			return new RewriteCondition(line, _values);
		}
		else if (IgnoredSettings.Any(x => line.StartsWith(x)))
		{
			//Ignored
			return null;
		}
		else
		{
			line.Dump();
		}
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


//Fuck, multiple Conditions then multiple Rules are allowed it seems...
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
		Conditions.All(c => c.Process(
	}
	public RedirectData ProcessRules(string url)
	{
	}
}


public class RewriteCondition : IRedirectLine
{
	static readonly Regex RewriteCondPattern = new Regex(@"^RewriteCond (%\{(?<variable>[^\}]+)\} )?(?<match>[^ ]+)$");
	public RewriteCondition(string line, ISampleValues values)
	{
		_values = values;
		
		var m = RewriteCondPattern.Match(line);
		
		if (!m.Success) throw new Exception("FAIL: '" + line + "'");
		
		if (m.Groups["variable"].Success)
			Variable = m.Groups["variable"].Value;
		MatchPattern = m.Groups["match"].Value;
	}
	
	readonly ISampleValues _values;
	
	public string Variable { get; set; }
	public string MatchPattern { get; set; }
	
	
	public bool WillProcess(RedirectData data)
	{
		throw new NotImplementedException();
	}
	public RedirectData Process(RedirectData data)
	{
//		for (int i = 0; i < Rules.Count; ++i)
//		{
//			if (data.Status == RedirectStatus.Continue)
//			{
//				data = Rules[i].Process(data);
//			}
//			if (data.Status == RedirectStatus.Redirected)
//			{
//				break;
//			}
//		}
//		return data;
		throw new NotImplementedException();
	}
	public bool IsCondition { get { return true; } }
	public bool IsRule { get { return false; } }
}
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
	
	
	public bool WillProcess(RedirectData data)
	{
		return true;
	}
	public RedirectData Process(RedirectData data)
	{
		throw new NotImplementedException();
	}
	public bool IsCondition { get { return false; } }
	public bool IsRule { get { return true; } }
}
[Flags]
public enum RuleOptions
{
	None = 0x00000000,
	[Text("NC")] //ISAPI
	[Text("I")] //Apache
	NoCase = 0x00000001,
	[Text("R=301")]
	Redirect301 = 0x00000010,
	[Text("F")]
	Forbidden = 0x00000100,
	[Text("L")]
	LastRule = 0x00001000,
	[Text("O")]
	Normalize = 0x00010000,
	[Text("NE")]
	///By default, special characters, such as & and ?, for example, will be converted to their hexcode equivalent. Using the [NE] flag prevents that from happening.
	NoEscape = 0x00100000,
	[Text("NU")]
	NoUnicode = 0x01000000,
	[Text("U")]
	///Log the URL as it was originally requested and not as the URL was rewritten.
	UnmangleLog = 0x01000000,
	[Text("CL")]
	//Changes the case of substitution result to lower.
	CaseLower = 0x10000000,
}
[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class TextAttribute : Attribute
{
	public TextAttribute(string value)
	{
		Value = value;
	}
	public string Value { get; set; }
}
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
				conf.Dump();
				throw new Exception("MISSING OPTION: " + o);
			}
		}
		
		return options;
	}
}



public interface IRedirectLine
{
	bool WillProcess(RedirectData data);
	RedirectData Process(RedirectData data);
	
	//TODO use enum??
	bool IsCondition { get; }
	bool IsRule { get; }
}
public class RedirectData
{
	public string OriginalUrl { get; set; }
	
	public string ProcessedUrl { get; set; }
	public RedirectStatus Status { get; set; }
	
	//TODO this goes in a RuleSetMatchData ?
	public List<string> ConditionMatchGroups { get; set; }
	public List<string> RuleMatchGroups { get; set; }
}
public enum RedirectStatus
{
	Continue,
	Redirected,
	//Restart
}





public static class MemberInfoExtensions
{
	public static IEnumerable<KeyValuePair<string, string>> OptionField(this MemberInfo method)
	{
		var attribs = method.GetCustomAttributes(typeof(TextAttribute), false)
							.Cast<TextAttribute>().ToArray();
		
		if (attribs.Length == 0) yield break;
		
		foreach (var a in attribs)
			yield return new KeyValuePair<string, string>(a.Value, method.Name);
	}
}
public static class AttributeExtensions
{
   /// <summary>
   /// Will return true if the attributeTarget is decorated with an attribute of type TAttribute.
   /// Will return false if not.
   /// </summary>
   /// <typeparam name="TAttribute"></typeparam>
   /// <param name="attributeTarget"></param>
   /// <returns></returns>
   public static bool IsDecoratedWith<TAttribute>(this ICustomAttributeProvider attributeTarget) where TAttribute : Attribute
   {
       return attributeTarget.GetCustomAttributes(typeof(TAttribute), false).Length > 0;
   }

}
