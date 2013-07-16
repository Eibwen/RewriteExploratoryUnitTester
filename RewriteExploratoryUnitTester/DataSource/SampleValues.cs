using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RewriteExploratoryUnitTester.DataSource
{
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

        private List<string> IpExamples = new List<string>
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
}
