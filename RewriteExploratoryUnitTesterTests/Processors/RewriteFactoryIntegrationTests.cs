using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using RewriteExploratoryUnitTester.Containers;
using RewriteExploratoryUnitTester.DataSource;
using RewriteExploratoryUnitTester.Processors;

namespace RewriteExploratoryUnitTesterTests.Processors
{
    [TestFixture]
    internal class RewriteFactoryIntegrationTests
    {
        [TestCase("http://movers.uship.com", true, true, "http://movers.uship.com/")]
        [TestCase("http://movers.uship.com?debug=1", true, true, "http://movers.uship.com/?debug=1")]
        [TestCase("http://movers.uship.com/?debug=1", true, false, "http://movers.uship.com/?debug=1")]
        [TestCase("http://movers.uship.com/test.ashx", true, false, null)]
        [TestCase("http://vehicles.uship.com/state/city", true, false, null)]
        public void Single_ruleset_should_manually_work_as_expected(string originalUrl, bool matchesCond, bool matchesRule, string expectedUrl)
        {
            //Arrange
            var samples = new RandomSampleValues();
            var factory = new RewriteFactory(samples);
            string[] lines =
                {
                    @"RewriteCond %{HTTP:Host} ^(movers|household-goods|vehicles|boats|motorcycles|special-care|freight|pets-livestock|food-agriculture|junk|craigslist)[2]?\.uship\.com$",
                    @"RewriteRule ^(?!.+\.axd|.+\.ashx|public/images|sticky/images)/([^?]*\u.*)/?(?:[^?]*\u.*)?$ /$1 [CL,R=301]"
                };
            var redirectData = new RedirectData(originalUrl);

            //Act
            var lineNum = 1;
            var redirects = lines.Select(l => factory.Build(lineNum++, l));

            //Assert
            var redirectLines = redirects as IList<IRedirectLine> ?? redirects.ToList();
            redirectLines.Count().Should().Be(2);

            var cond = (RewriteCondition) redirectLines.Single(x => x.LineType == RedirectLineType.Condition);
            cond.Variable.Should().Be("HTTP:Host");
            cond.MatchesCondition(ref redirectData).Should().Be(matchesCond);
            if (matchesCond)
            {
                var rule = (RewriteRule) redirectLines.Single(x => x.LineType == RedirectLineType.Rule);
                redirectData = rule.ProcessRule(redirectData);
                if (matchesRule)
                {
                    redirectData.Status.Should().NotBe(RedirectStatus.NotProcessed);
                    redirectData.ProcessedUrl.Should().Be(expectedUrl);
                }
                else
                {
                    redirectData.Status.Should().Be(RedirectStatus.NotProcessed);
                }
            }
        }

        [TestCase("http://movers.uship.com", true, true, "http://movers.uship.com/")]
        [TestCase("http://movers.uship.com?debug=1", true, true, "http://movers.uship.com/?debug=1")]
        [TestCase("http://movers.uship.com/?debug=1", true, false, "http://movers.uship.com/?debug=1")]
        [TestCase("http://movers.uship.com/test.ashx", true, false, null)]
        [TestCase("http://vehicles.uship.com/state/city", true, false, null)]
        public void Single_ruleset_should_object_work_as_expected(string originalUrl, bool matchesCond, bool matchesRule, string expectedUrl)
        {
            //Arrange
            var samples = new RandomSampleValues();
            var factory = new RewriteFactory(samples);
            string[] lines =
                {
                    @"RewriteCond %{HTTP:Host} ^(movers|household-goods|vehicles|boats|motorcycles|special-care|freight|pets-livestock|food-agriculture|junk|craigslist)[2]?\.uship\.com$",
                    @"RewriteRule ^(?!.+\.axd|.+\.ashx|public/images|sticky/images)/([^?]*\u.*)/?(?:[^?]*\u.*)?$ /$1 [CL,R=301]"
                };
            var lineNum = 1;
            var redirects = lines.Select(l => factory.Build(lineNum++, l));
            var ruleSet = RewriteRuleSet.BuildRuleSets(redirects);
            var redirectData = new RedirectData(originalUrl);

            //Act
            var matchingSets = ruleSet.Where(r => r.ProcessConditions(ref redirectData));
            var rewriteRuleSets = matchingSets as IList<RewriteRuleSet> ?? matchingSets.ToList();
            redirectData = rewriteRuleSets.First().ProcessRules(redirectData);

            //Assert
            if (matchesCond)
            {
                rewriteRuleSets.Count().Should().Be(1);

                if (matchesRule)
                {
                    redirectData.Status.Should().NotBe(RedirectStatus.NotProcessed);
                    redirectData.ProcessedUrl.Should().Be(expectedUrl);
                }
                else
                {
                    redirectData.Status.Should().Be(RedirectStatus.NotProcessed);
                }
            }
            else
            {
                rewriteRuleSets.Count().Should().Be(0);
            }
        }

        [TestCase("http://vehicles.uship.com/state", true, true, "http://www.uship.com/vehicles/state")]
        [TestCase("http://vehicles.uship.com/state/", true, true, "http://www.uship.com/vehicles/state/")]
        [TestCase("http://vehicles.uship.com/state?debug=1", true, true, "http://www.uship.com/vehicles/state?debug=1")]
        [TestCase("http://vehicles.uship.com/state/city", true, true, "http://www.uship.com/vehicles/state/city")]
        [TestCase("http://vehicles.uship.com/state/city/", true, true, "http://www.uship.com/vehicles/state/city/")]
        [TestCase("http://vehicles.uship.com/state/city/?debug=1", true, true, "http://www.uship.com/vehicles/state/city/?debug=1")]
        [TestCase("motorcycles.uship.com/state/city/", true, true, "http://www.uship.com/motorcycles/state/city/")]
        [TestCase("http://www.uship.com/motorcycles/texas/austin/", false, false, null)]
        [TestCase("http://www.uship.com/motorcycles/texas/austin", false, false, null)]
        [TestCase("http://motorcycles.uship.com/texas/austin/", true, true, "http://www.uship.com/motorcycles/texas/austin/")]
        [TestCase("http://motorcycles.uship.com/texas/austin", true, true, "http://www.uship.com/motorcycles/texas/austin")]
        [TestCase("http://MoToRcYcLeS.UsHiP.com/Texas/AuStIn/", true, true, "http://www.uship.com/motorcycles/texas/austin/")]
        [TestCase("http://household-goods.uship.com/texas/austin", true, true, "http://www.uship.com/furniture/texas/austin")]
        [TestCase("http://special-care.uship.com/texas/austin", true, true, "http://www.uship.com/special-care/")]
        [TestCase("http://food-agriculture.uship.com/texas/austin", true, true, "http://www.uship.com/food/")]
        public void Vortal_rewrites_alone_should_go_to_correct_domain(string originalUrl, bool matchesCond, bool matchesRule, string expectedUrl)
        {
            //Arrange
            var samples = new RandomSampleValues();
            var factory = new RewriteFactory(samples);
            string[] lines =
                {
                    @"RewriteCond %{HTTP:Host} ^(movers|vehicles|boats|motorcycles|freight)\.uship\.com$",
                    @"RewriteRule ^/(.+)$ http://www.uship.com/%1/$1 [CL,NC,R=301]",
                    @"",
                    @"RewriteCond %{HTTP:Host} ^pets-livestock\.uship\.com$",
                    @"RewriteRule ^/(.+)$ http://www.uship.com/pet-shipping/$1 [CL,NC,R=301]",
                    @"",
                    @"RewriteCond %{HTTP:Host} ^household-goods\.uship\.com$",
                    @"RewriteRule ^/(.+)$ http://www.uship.com/furniture/$1 [CL,NC,R=301]",
                    @"",
                    @"# 301 these lortals to their vortals, these lortals will not exist",
                    @"RewriteCond %{HTTP:Host} ^(craigslist|special-care|junk|food)(?:-agriculture)?\.uship\.com$",
                    @"RewriteRule ^/(.+)$ http://www.uship.com/%1/ [CL,NC,R=301]"
                };
            var lineNum = 1;
            var redirects = lines.Select(l => factory.Build(lineNum++, l));
            var ruleSet = RewriteRuleSet.BuildRuleSets(redirects);
            var redirectData = new RedirectData(originalUrl);

            //Act
            var matchingSets = ruleSet.Where(r => r.ProcessConditions(ref redirectData));
            var rewriteRuleSets = matchingSets as IList<RewriteRuleSet> ?? matchingSets.ToList();

            //Assert
            if (matchesCond)
            {
                rewriteRuleSets.Count().Should().Be(1);
                redirectData = rewriteRuleSets.First().ProcessRules(redirectData);

                if (matchesRule)
                {
                    redirectData.Status.Should().NotBe(RedirectStatus.NotProcessed);
                    redirectData.ProcessedUrl.Should().Be(expectedUrl);
                }
                else
                {
                    redirectData.Status.Should().Be(RedirectStatus.NotProcessed);
                }
            }
            else
            {
                rewriteRuleSets.Count().Should().Be(0);
            }
        }
    }
}
