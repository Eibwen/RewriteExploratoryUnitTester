using System;
using System.Collections.Generic;
using RewriteExploratoryUnitTester.DataSource;

namespace RewriteExploratoryUnitTester.Containers
{
    public class RedirectData
    {
        private string _processedUrl;

        public RedirectData(string url)
        {
            OriginalUrl = CleanUrl(url);
        }

//        public static RedirectData Create(string url)
//        {
//            var data = new RedirectData();
//            data.OriginalUrl = CleanUrl(url);
//            return data;
//        }

        public virtual Uri CleanUrl(string url)
        {
            if (url.StartsWith("http://") || url.StartsWith("https://"))
            {
                return new Uri(url);
            }
            else
            {
                return new Uri("http://" + url);
            }
        }

        public Uri OriginalUrl { get; set; }

        public string ProcessedUrl
        {
            get
            {
                if (_processedUrl == null) return OriginalUrl.OriginalString;

                return Uri.IsWellFormedUriString(_processedUrl, UriKind.Absolute)
                    ? _processedUrl
                    : new Uri(OriginalUrl, _processedUrl).ToString();
            }
            set { _processedUrl = value; }
        }

        internal string CurrentPathAndQuery
        {
            get
            {
                if (_processedUrl != null)
                {
                    return new Uri(ProcessedUrl).PathAndQuery;
                }
                return OriginalUrl.PathAndQuery;
            }
        }

        public void SetUrlChanged()
        {
            WasRedirected = true;
        }

        /// <summary>
        /// Denotes the status of the LATEST RULE PROCESSED AGAINST THIS DATA
        /// </summary>
        public RedirectStatus Status { get; set; }
        public bool WasRedirected { get; private set; }

        //TODO this goes in a RuleSetMatchData ?
        public List<string> ConditionMatchGroups { get; set; }
        public List<string> RuleMatchGroups { get; set; }

        public RewriteRuleSet RuleSetMatched
        {
            get;
            set;
        }
    }
}
