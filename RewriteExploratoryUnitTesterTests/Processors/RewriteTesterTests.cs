using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using RewriteExploratoryUnitTester.DataSource;
using RewriteExploratoryUnitTester.HelperClasses;
using RewriteExploratoryUnitTester.Processors;

namespace RewriteExploratoryUnitTesterTests.Processors
{
    [TestFixture, Ignore("Using my own rewrite config")]
    public class RewriteTesterTests
    {
        [TestCase("https://dev.uship.com/ltl-freight/", "https://dev.uship.com/vortals/vortal2.aspx?siteID=1&directory=ltl-freight&alwaysShowCCTA=1&")]
        public void ltl_freight_full_file(string originalUrl, string expectedUrl)
        {
            //Arrange
            var samples = new RandomSampleValues();
            var factory = new RewriteFactory(samples);
            var tester = new RewriteTester(factory);

            var configFile = @"...\httpd.conf";

            tester.LoadConfig(configFile);

            //Act
            var output = tester.TestUrl(originalUrl);

            //Assert
            output.ProcessedUrl.Should().Be(expectedUrl);
            output.RuleSetMatched.Rules.First().ReplacePattern.Should().Contain("vortal");
        }
    }
}