using FluentAssertions;
using NUnit.Framework;
using RewriteExploratoryUnitTester.Containers;
using RewriteExploratoryUnitTester.DataSource;
using RewriteExploratoryUnitTester.Processors;

namespace RewriteExploratoryUnitTesterTests.Processors
{
    [TestFixture]
    class RewriteFactoryTests
    {
        RewriteFactory ClassUnderTest
        {
            get
            {
                var samples = new RandomSampleValues();
                var factory = new RewriteFactory(samples);
                return factory;
            }
        }

        [TestCase("RewriteEngine")]
        [TestCase("RewriteCompatibility2")]
        [TestCase("RepeatLimit")]
        [TestCase("RewriteBase")]
        [TestCase("")]
        [TestCase(null)]
        [TestCase("#comment")]
        public void Currently_ignore_other_line_types(string lineStart)
        {
            //Arrange

            //Act
            var rewrite = ClassUnderTest.Build(1, lineStart);

            //Assert
            rewrite.Should().BeNull();
        }

        [Test]
        public void RewriteCond_should_be_parsed()
        {
            //Arrange
            var line = @"RewriteCond ^.*widget_artfact.*$";

            //Act
            var rewrite = ClassUnderTest.Build(235, line);

            //Assert
            rewrite.LineType.Should().Be(RedirectLineType.Condition);
            var castRewrite = (RewriteCondition)rewrite;
            castRewrite.LineNumber.Should().Be(235);
            castRewrite.MatchPattern.Should().Be(@"^.*widget_artfact.*$");
            castRewrite.Variable.Should().BeNull();
        }

        [Test]
        public void RewriteCond_variable_should_be_parsed()
        {
            //Arrange
            var line = @"RewriteCond %{HTTP:Host} ^(movers|household-goods|vehicles|boats|motorcycles|special-care|freight|pets-livestock|food-agriculture|junk|craigslist)[2]?\.uship\.com$";

            //Act
            var rewrite = ClassUnderTest.Build(1, line);

            //Assert
            rewrite.LineType.Should().Be(RedirectLineType.Condition);
            var castRewrite = (RewriteCondition)rewrite;
            castRewrite.LineNumber.Should().Be(1);
            castRewrite.MatchPattern.Should().Be(@"^(movers|household-goods|vehicles|boats|motorcycles|special-care|freight|pets-livestock|food-agriculture|junk|craigslist)[2]?\.uship\.com$");
            castRewrite.Variable.Should().Be("HTTP:Host");
        }

        [Test]
        public void RewriteRule_should_be_parsed()
        {
            //Arrange
            var line = @"RewriteRule ^(?!.+\.axd|.+\.ashx|public/images|sticky/images)/([^?]*\u.*)/?(?:[^?]*\u.*)?$ /$1 [CL,R=301]";

            //Act
            var rewrite = ClassUnderTest.Build(2, line);

            //Assert
            rewrite.LineType.Should().Be(RedirectLineType.Rule);
            var castRewrite = (RewriteRule)rewrite;
            castRewrite.LineNumber.Should().Be(2);
            castRewrite.MatchPattern.Should().Be(@"^(?!.+\.axd|.+\.ashx|public/images|sticky/images)/([^?]*\u.*)/?(?:[^?]*\u.*)?$".Replace(@"\u", ""));
            castRewrite.ReplacePattern.Should().Be("/$1");
            castRewrite.Options.Should().Be(RuleOptions.CaseLower | RuleOptions.Redirect301);
        }
    }
}
