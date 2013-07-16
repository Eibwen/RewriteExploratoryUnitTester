using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RewriteExploratoryUnitTester.Containers;

namespace RewriteExploratoryUnitTester.DataSource
{
    public interface ISampleValues
    {
        string GetSampleValue(string variable, RedirectData data);
    }

    public class RandomSampleValues : ISampleValues
    {
        private readonly Dictionary<string, Func<RedirectData, string>> LookupDictionary;

        public RandomSampleValues()
        {
            LookupDictionary = null;
        }

        public RandomSampleValues(Dictionary<string, string> values)
        {
            LookupDictionary = values.ToDictionary(k => k.Key,
                                                   v => new Func<RedirectData, string>(r => v.Value));
        }

        public RandomSampleValues(Dictionary<string, Func<RedirectData, string>> values)
        {
            LookupDictionary = values;
        }

        //TODO would want this for unit tests to be easily loaded from a collection...
        public virtual string GetSampleValue(string variable, RedirectData data)
        {
            if (LookupDictionary != null && LookupDictionary.ContainsKey(variable))
            {
                return LookupDictionary[variable](data);
            }

            switch (variable)
            {
                case "HTTP_TRUE_CLIENT_IP":
                    return GetRandom(IpExamples);
                case "HTTP_HOST":
                case "HTTP:Host":
                    return data.OriginalUrl.Host;
                case "HTTP:User-Agent":
                    return GetRandom(UserAgentExamples);
                case "REQUEST_METHOD":
                    return "GET";
                case "HTTPS":
                    return data.OriginalUrl.OriginalString.StartsWith("https") ? "ON" : "OFF";
                case "SERVER_PORT":
                    return data.OriginalUrl.Port.ToString();
                case "QUERY_STRING":
                    return data.OriginalUrl.Query;
                case "HTTP:Referer":
                    return GetRandom(ReferrerExamples);
                case "REQUEST_URI":
                    return data.OriginalUrl.OriginalString;
            }
            throw new Exception("Unknown variable: " + variable);
        }

        private readonly Random rand = new Random();

        private string GetRandom(List<string> source)
        {
            return source[rand.Next(0, source.Count)];
        }

        public List<string> IpExamples = new List<string>
            {
                "192.168.5.155",
                //"166.78.137.146"
            };

        public List<string> ReferrerExamples = new List<string>
            {
                "http://www.uship.com"
            };

        public List<string> UserAgentExamples = new List<string>
            {
                "",

            };
    }
}
