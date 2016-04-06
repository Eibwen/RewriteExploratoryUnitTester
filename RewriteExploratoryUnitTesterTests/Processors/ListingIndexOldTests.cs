using System;
using System.Linq;
using NUnit.Framework;

namespace RewriteExploratoryUnitTesterTests.Processors
{
    [TestFixture]
    class ListingIndexOldTests
    {
        [Ignore("Still a work in progress")]
        [TestCase("http://www.uship.com/cost-to-ship", true, true, "http://www.uship.com/listingindex/?siteID=1")]
        public void Grand_test_of_all_ness(string originalUrl, bool matchesCond, bool matchesRule, string expectedUrl)
        {
            //These two should 100% match!
            OriginalRules(originalUrl, matchesCond, matchesRule, expectedUrl);
            Console.WriteLine("- OLD RULE SUCCESSFUL -");
            Console.WriteLine();
            UsingFallThroughRules(originalUrl, matchesCond, matchesRule, expectedUrl);
        }

        private void OriginalRules(string originalUrl, bool matchesCond, bool matchesRule, string expectedUrl)
        {
            //Arrange
            var configSection = @"##  Listing Index  ##
# US
RewriteRule ^/cost-to-ship/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&c4=$4&page=$5&siteID=1 [NC,L]
RewriteRule ^/cost-to-ship/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&page=$4&siteID=1 [NC,L]
RewriteRule ^/cost-to-ship/([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&page=$3&siteID=1 [NC,L]
RewriteRule ^/cost-to-ship/([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&page=$2&siteID=1 [NC,L]
RewriteRule ^/cost-to-ship/?$ /listingindex/?siteID=1 [NC,L]

# United Kingdom - /uk/cost-to-ship/  3
RewriteRule ^/uk/cost-to-ship/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&c4=$4&page=$5&siteID=3 [NC,L]
RewriteRule ^/uk/cost-to-ship/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&page=$4&siteID=3 [NC,L]
RewriteRule ^/uk/cost-to-ship/([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&page=$3&siteID=3 [NC,L]
RewriteRule ^/uk/cost-to-ship/([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&page=$2&siteID=3 [NC,L]
RewriteRule ^/uk/cost-to-ship/?$ /listingindex/?siteID=3 [NC,L]

# Australia - /au/cost-to-ship/  5
RewriteRule ^/au/cost-to-ship/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&c4=$4&page=$5&siteID=5 [NC,L]
RewriteRule ^/au/cost-to-ship/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&page=$4&siteID=5 [NC,L]
RewriteRule ^/au/cost-to-ship/([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&page=$3&siteID=5 [NC,L]
RewriteRule ^/au/cost-to-ship/([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&page=$2&siteID=5 [NC,L]
RewriteRule ^/au/cost-to-ship/?$ /listingindex/?siteID=5 [NC,L]

# Canada - /ca/cost-to-ship/  2
RewriteRule ^/ca/cost-to-ship/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&c4=$4&page=$5&siteID=2 [NC,L]
RewriteRule ^/ca/cost-to-ship/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&page=$4&siteID=2 [NC,L]
RewriteRule ^/ca/cost-to-ship/([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&page=$3&siteID=2 [NC,L]
RewriteRule ^/ca/cost-to-ship/([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&page=$2&siteID=2 [NC,L]
RewriteRule ^/ca/cost-to-ship/?$ /listingindex/?siteID=2 [NC,L]

# India - /in/cost-to-ship/  6
RewriteRule ^/in/cost-to-ship/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&c4=$4&page=$5&siteID=6 [NC,L]
RewriteRule ^/in/cost-to-ship/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&page=$4&siteID=6 [NC,L]
RewriteRule ^/in/cost-to-ship/([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&page=$3&siteID=6 [NC,L]
RewriteRule ^/in/cost-to-ship/([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&page=$2&siteID=6 [NC,L]
RewriteRule ^/in/cost-to-ship/?$ /listingindex/?siteID=6 [NC,L]

# South Africa - /za/cost-to-ship/  15
RewriteRule ^/za/cost-to-ship/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&c4=$4&page=$5&siteID=15 [NC,L]
RewriteRule ^/za/cost-to-ship/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&page=$4&siteID=15 [NC,L]
RewriteRule ^/za/cost-to-ship/([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&page=$3&siteID=15 [NC,L]
RewriteRule ^/za/cost-to-ship/([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&page=$2&siteID=15 [NC,L]
RewriteRule ^/za/cost-to-ship/?$ /listingindex/?siteID=15 [NC,L]

# European Union - /eu/cost-to-ship/  4
RewriteRule ^/eu/cost-to-ship/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&c4=$4&page=$5&siteID=4 [NC,L]
RewriteRule ^/eu/cost-to-ship/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&page=$4&siteID=4 [NC,L]
RewriteRule ^/eu/cost-to-ship/([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&page=$3&siteID=4 [NC,L]
RewriteRule ^/eu/cost-to-ship/([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&page=$2&siteID=4 [NC,L]
RewriteRule ^/eu/cost-to-ship/?$ /listingindex/?siteID=4 [NC,L]

# Germany
RewriteRule ^/de/transportkosten/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&c4=$4&page=$5&siteID=8 [NC,L]
RewriteRule ^/de/transportkosten/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&page=$4&siteID=8 [NC,L]
RewriteRule ^/de/transportkosten/([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&page=$3&siteID=8 [NC,L]
RewriteRule ^/de/transportkosten/([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&page=$2&siteID=8 [NC,L]
RewriteRule ^/de/transportkosten/?$ /listingindex/?siteID=8 [NC,L]

# Netherlands - /nl/transportkosten/  10
RewriteRule ^/nl/transportkosten/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&c4=$4&page=$5&siteID=10 [NC,L]
RewriteRule ^/nl/transportkosten/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&page=$4&siteID=10 [NC,L]
RewriteRule ^/nl/transportkosten/([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&page=$3&siteID=10 [NC,L]
RewriteRule ^/nl/transportkosten/([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&page=$2&siteID=10 [NC,L]
RewriteRule ^/nl/transportkosten/?$ /listingindex/?siteID=10 [NC,L]

# Austria - /at/transportkosten/   19
RewriteRule ^/at/transportkosten/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&c4=$4&page=$5&siteID=19 [NC,L]
RewriteRule ^/at/transportkosten/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&page=$4&siteID=19 [NC,L]
RewriteRule ^/at/transportkosten/([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&page=$3&siteID=19 [NC,L]
RewriteRule ^/at/transportkosten/([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&page=$2&siteID=19 [NC,L]
RewriteRule ^/at/transportkosten/?$ /listingindex/?siteID=19 [NC,L]

# France - /fr/le-coût-du-transport/  9
RewriteRule ^/fr/le-coût-du-transport/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&c4=$4&page=$5&siteID=9 [NC,L]
RewriteRule ^/fr/le-coût-du-transport/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&page=$4&siteID=9 [NC,L]
RewriteRule ^/fr/le-coût-du-transport/([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&page=$3&siteID=9 [NC,L]
RewriteRule ^/fr/le-coût-du-transport/([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&page=$2&siteID=9 [NC,L]
RewriteRule ^/fr/le-coût-du-transport/?$ /listingindex/?siteID=9 [NC,L]

# Spain - /es/precio-de-envío/  11
RewriteRule ^/es/precio-de-envío/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&c4=$4&page=$5&siteID=11 [NC,L]
RewriteRule ^/es/precio-de-envío/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&page=$4&siteID=11 [NC,L]
RewriteRule ^/es/precio-de-envío/([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&page=$3&siteID=11 [NC,L]
RewriteRule ^/es/precio-de-envío/([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&page=$2&siteID=11 [NC,L]
RewriteRule ^/es/precio-de-envío/?$ /listingindex/?siteID=11 [NC,L]

# Mexico - /mx/precio-de-envío/  14
RewriteRule ^/mx/precio-de-envío/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&c4=$4&page=$5&siteID=14 [NC,L]
RewriteRule ^/mx/precio-de-envío/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&page=$4&siteID=14 [NC,L]
RewriteRule ^/mx/precio-de-envío/([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&page=$3&siteID=14 [NC,L]
RewriteRule ^/mx/precio-de-envío/([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&page=$2&siteID=14 [NC,L]
RewriteRule ^/mx/precio-de-envío/?$ /listingindex/?siteID=14 [NC,L]

# Argentina - /ar/precio-de-envío/  22
RewriteRule ^/ar/precio-de-envío/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&c4=$4&page=$5&siteID=22 [NC,L]
RewriteRule ^/ar/precio-de-envío/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&page=$4&siteID=22 [NC,L]
RewriteRule ^/ar/precio-de-envío/([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&page=$3&siteID=22 [NC,L]
RewriteRule ^/ar/precio-de-envío/([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&page=$2&siteID=22 [NC,L]
RewriteRule ^/ar/precio-de-envío/?$ /listingindex/?siteID=22 [NC,L]

# Chile - /cl/precio-de-envío/  23
RewriteRule ^/cl/precio-de-envío/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&c4=$4&page=$5&siteID=23 [NC,L]
RewriteRule ^/cl/precio-de-envío/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&page=$4&siteID=23 [NC,L]
RewriteRule ^/cl/precio-de-envío/([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&page=$3&siteID=23 [NC,L]
RewriteRule ^/cl/precio-de-envío/([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&page=$2&siteID=23 [NC,L]
RewriteRule ^/cl/precio-de-envío/?$ /listingindex/?siteID=23 [NC,L]

# Colombia - /co/precio-de-envío/  27
RewriteRule ^/co/precio-de-envío/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&c4=$4&page=$5&siteID=27 [NC,L]
RewriteRule ^/co/precio-de-envío/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&page=$4&siteID=27 [NC,L]
RewriteRule ^/co/precio-de-envío/([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&page=$3&siteID=27 [NC,L]
RewriteRule ^/co/precio-de-envío/([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&page=$2&siteID=27 [NC,L]
RewriteRule ^/co/precio-de-envío/?$ /listingindex/?siteID=27 [NC,L]

# Peru - /pe/precio-de-envío/  26
RewriteRule ^/pe/precio-de-envío/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&c4=$4&page=$5&siteID=26 [NC,L]
RewriteRule ^/pe/precio-de-envío/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&page=$4&siteID=26 [NC,L]
RewriteRule ^/pe/precio-de-envío/([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&page=$3&siteID=26 [NC,L]
RewriteRule ^/pe/precio-de-envío/([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&page=$2&siteID=26 [NC,L]
RewriteRule ^/pe/precio-de-envío/?$ /listingindex/?siteID=26 [NC,L]

# Venezuela - /ve/precio-de-envío/  28
RewriteRule ^/ve/precio-de-envío/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&c4=$4&page=$5&siteID=28 [NC,L]
RewriteRule ^/ve/precio-de-envío/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&page=$4&siteID=28 [NC,L]
RewriteRule ^/ve/precio-de-envío/([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&page=$3&siteID=28 [NC,L]
RewriteRule ^/ve/precio-de-envío/([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&page=$2&siteID=28 [NC,L]
RewriteRule ^/ve/precio-de-envío/?$ /listingindex/?siteID=28 [NC,L]

# Brazil - /br/custo-de-transporte/  21
RewriteRule ^/br/custo-de-transporte/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&c4=$4&page=$5&siteID=21 [NC,L]
RewriteRule ^/br/custo-de-transporte/([^/]+)/(?!page)([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&c3=$3&page=$4&siteID=21 [NC,L]
RewriteRule ^/br/custo-de-transporte/([^/]+)/(?!page)([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&c2=$2&page=$3&siteID=21 [NC,L]
RewriteRule ^/br/custo-de-transporte/([^/]+)(?:/page/([0-9]+))? /listingindex/PricingCommodity.aspx?&c1=$1&page=$2&siteID=21 [NC,L]
RewriteRule ^/br/custo-de-transporte/?$ /listingindex/?siteID=21 [NC,L]

##  End Listing Index  ##";
            var lines = configSection.Split('\n').Select(x => x.Trim());

            //Act & /Assert
            RewriteFactoryIntegrationTests.BuildAndTestConditions(lines, originalUrl, matchesCond, matchesRule, expectedUrl);
        }

        private void UsingFallThroughRules(string originalUrl, bool matchesCond, bool matchesRule, string expectedUrl)
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