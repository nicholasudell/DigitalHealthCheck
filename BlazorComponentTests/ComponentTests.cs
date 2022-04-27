using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bunit;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace BlazorComponentTests
{
    public class GDSComponentTests : BunitTestContext
    {
        static readonly IDictionary<string, IComponentFactory> fixtures = new Dictionary<string, IComponentFactory>()
        {
            {"button", new ButtonFactory()},
            {"input", new InputFactory()},
            {"label", new LabelFactory()},
            {"error-message", new ErrorMessageFactory()},
            {"hint", new HintFactory()},
            {"radios", new RadiosFactory()},
            {"fieldset", new FieldSetFactory()},
            {"details", new DetailsFactory() },
            {"checkboxes", new CheckboxesFactory() },
            {"date-input", new DateInputFactory() },
            {"summary-list", new SummaryListFactory() },
            {"panel", new PanelFactory() },
            {"tag", new TagFactory()},
            {"accordion", new AccordionFactory()},
            {"error-summary", new ErrorSummaryFactory()},
            {"textarea", new TextAreaFactory()},
            {"character-count", new CharacterCountFactory()},
            {"cookie-banner", new CookieBannerFactory()},
            {"notification-banner", new NotificationBannerFactory()}
        };

        static string FixturesDirectory => Path.Combine(NUnit.Framework.TestContext.CurrentContext.TestDirectory, "Fixtures");

        [TestCaseSource(nameof(TestData))]
        public void ComponentConformsToGDSSpec(JObject options, string expectedHtml, IComponentFactory componentFactory)
        {
            try
            {
                var component = componentFactory.CreateComponent(TestContext, options);

                component.MarkupMatches(expectedHtml, $"Component verification failed.{Environment.NewLine}Options:{Environment.NewLine}{CreateOptionsReadout(options)}");

                Console.WriteLine($"Options:{Environment.NewLine}{CreateOptionsReadout(options)}{Environment.NewLine}{Environment.NewLine}Correct markup:{Environment.NewLine}{expectedHtml}");
            }
            catch (Exception e) when (e is not Bunit.HtmlEqualException)
            {
                throw new Exception($"Component verification failed.{Environment.NewLine}Options:{Environment.NewLine}{CreateOptionsReadout(options)}", e);
            }
        }

        static string CreateFullFixturePath(string componentName) => Path.Combine
                (
            FixturesDirectory,
            componentName,
            "fixtures.json"
        );

        static string CreateOptionsReadout(JObject options) =>
                string.Join(Environment.NewLine, options.Properties().Select(x => $"{x.Name}: {x.Value}"));

        static JObject LoadFixtureFile(string path) => JObject.Parse(File.ReadAllText(path));

        static IEnumerable<TestCaseData> TestData()
                            => fixtures.SelectMany(x =>
            {
                var component = LoadFixtureFile(CreateFullFixturePath(x.Key));

                return component["fixtures"].Select(fixture =>
                {
                    var componentName = component.Value<string>("component");
                    var testName = $"{componentName}: {fixture.Value<string>("name").Replace("=", "")}";
                    var options = (JObject)fixture["options"];

                    var testCase = new TestCaseData(
                        options,
                        fixture.Value<string>("html"),
                        x.Value);

                    testCase.SetDescription($"The GDS {componentName} component should match the expected GDS fixture data when the following options are provided:{Environment.NewLine}{CreateOptionsReadout(options)}");
                    testCase.SetName(testName);

                    return testCase;
                });
            });
    }
}