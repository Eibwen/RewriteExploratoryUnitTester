using System.Linq;
using NUnit.Framework;

namespace RewriteExploratoryUnitTesterTests.Processors
{
    [TestFixture]
    class ListingIndexOldTests
    {
        [TestCase("http://www.uship.com/cost-to-ship", true, true, "http://www.uship.com/listingindex/PricingCommodity.aspx?&c1=?&page=&siteID=1")]
        public void Grand_test_of_all_ness(string originalUrl, bool matchesCond, bool matchesRule, string expectedUrl)
        {
            //Arrange
            var configSection = @"##  Listing Index  ##
# US
RewriteRule ^/cost-to-ship/?(.*)$ /listingindexSiteDetected/$1?siteID=1 [NC]

# United Kingdom - /uk/cost-to-ship/  3
RewriteRule ^/uk/cost-to-ship/?(.*)$ /listingindexSiteDetected/$1?siteID=3 [NC]

# Australia - /au/cost-to-ship/  5
RewriteRule ^/au/cost-to-ship/?(.*)$ /listingindexSiteDetected/$1?siteID=5 [NC]

# Canada - /ca/cost-to-ship/  2
RewriteRule ^/ca/cost-to-ship/?(.*)$ /listingindexSiteDetected/$1?siteID=2 [NC]

# India - /in/cost-to-ship/  6
RewriteRule ^/in/cost-to-ship/?(.*)$ /listingindexSiteDetected/$1?siteID=6 [NC]

# South Africa - /za/cost-to-ship/  15
RewriteRule ^/za/cost-to-ship/?(.*)$ /listingindexSiteDetected/$1?siteID=15 [NC]

# European Union - /eu/cost-to-ship/  4
RewriteRule ^/eu/cost-to-ship/?(.*)$ /listingindexSiteDetected/$1?siteID=4 [NC]

# Germany
RewriteRule ^/de/transportkosten/?(.*)$ /listingindexSiteDetected/$1?siteID=8 [NC]

# Netherlands - /nl/transportkosten/  10
RewriteRule ^/nl/transportkosten/?(.*)$ /listingindexSiteDetected/$1?siteID=10 [NC]

# Austria - /at/transportkosten/   19
RewriteRule ^/at/transportkosten/?(.*)$ /listingindexSiteDetected/$1?siteID=19 [NC]

# France - /fr/le-coût-du-transport/  9
RewriteRule ^/fr/le-coût-du-transport/?(.*)$ /listingindexSiteDetected/$1?siteID=9 [NC]

# Spain - /es/precio-de-envío/  11
RewriteRule ^/es/precio-de-envío/?(.*)$ /listingindexSiteDetected/$1?siteID=11 [NC]

# Mexico - /mx/precio-de-envío/  14
RewriteRule ^/mx/precio-de-envío/?(.*)$ /listingindexSiteDetected/$1?siteID=14 [NC]

# Argentina - /ar/precio-de-envío/  22
RewriteRule ^/ar/precio-de-envío/?(.*)$ /listingindexSiteDetected/$1?siteID=22 [NC]

# Chile - /cl/precio-de-envío/  23
RewriteRule ^/cl/precio-de-envío/?(.*)$ /listingindexSiteDetected/$1?siteID=23 [NC]

# Colombia - /co/precio-de-envío/  27
RewriteRule ^/co/precio-de-envío/?(.*)$ /listingindexSiteDetected/$1?siteID=27 [NC]

# Peru - /pe/precio-de-envío/  26
RewriteRule ^/pe/precio-de-envío/?(.*)$ /listingindexSiteDetected/$1?siteID=26 [NC]

# Venezuela - /ve/precio-de-envío/  28
RewriteRule ^/ve/precio-de-envío/?(.*)$ /listingindexSiteDetected/$1?siteID=28 [NC]

# Brazil - /br/custo-de-transporte/  21
RewriteRule ^/br/custo-de-transporte/?(.*)$ /listingindexSiteDetected/$1?siteID=21 [NC]

RewriteRule ^/listingindexSiteDetected/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))?/?\??siteID=(\d+) /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&c4=$4&page=$5&siteID=$6 [NC,L]
RewriteRule ^/listingindexSiteDetected/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))?/?\??siteID=(\d+) /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&page=$4&siteID=$5 [NC,L]
RewriteRule ^/listingindexSiteDetected/([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))?/?\??siteID=(\d+) /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&page=$3&siteID=$4 [NC,L]
RewriteRule ^/listingindexSiteDetected/([^/]+)(?:/page/([0-9]+))?/?\??siteID=(\d+) /listingindex/PricingCommodity.aspx?&c1=$1&page=$2&siteID=$3 [NC,L]
RewriteRule ^/listingindexSiteDetected/?/?\??siteID=(\d+)$ /listingindex/?siteID=$1 [NC,L]

##  End Listing Index  ##";
            var lines = configSection.Split('\n').Select(x => x.Trim());

            //Act & /Assert
            RewriteFactoryIntegrationTests.BuildAndTestConditions(lines, originalUrl, matchesCond, matchesRule, expectedUrl);
        }
    }
}