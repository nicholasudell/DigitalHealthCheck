using System.Collections.Generic;
using System.Linq;
using Bunit;
using DigitalHealthCheckWeb.Components.GDS;
using Newtonsoft.Json.Linq;

namespace BlazorComponentTests
{
    public class AccordionFactory : ComponentFactory<GDSAccordion>
    {
        public static GDSAccordion.Options GetAccordion(JObject options)
        {
            if (options is null)
            {
                return null;
            }

            return new GDSAccordion.Options
            {
                Classes = options.Value<string>("classes"),
                HeadingLevel = options.Value<int?>("headingLevel"),
                Id = options.Value<string>("id"),
                Items = GetItems((JArray)options["items"]).ToList()
            };
        }

        protected override ComponentParameterCollectionBuilder<GDSAccordion> AssignParameters
                (
            ComponentParameterCollectionBuilder<GDSAccordion> parameters,
            JObject options
        )
        {
            var accordion = GetAccordion(options);

            parameters.Add(x => x.Classes, accordion.Classes);
            parameters.Add(x => x.HeadingLevel, accordion.HeadingLevel);
            parameters.Add(x => x.Id, accordion.Id);
            parameters.Add(x => x.Items, accordion.Items);

            return parameters;
        }

        private static IEnumerable<GDSAccordion.Item> GetItems(JArray items)
        {
            if (items is null)
            {
                yield break;
            }

            static GDSAccordion.Item GetItem(JObject options)
            {
                var conditionalContentString = options["conditional"]?.Value<string>("html");

                // Kludge to fix a test for options intended to be invalid that would be valid as a
                // RenderFragment. The test assumes that "false" is equivalent to absence of data,
                // even though the input here is a RenderFragment, and a fragment containing the
                // string "False" is neither null nor invalid content.

                if (conditionalContentString == "False")
                {
                    conditionalContentString = null;
                }

                return new GDSAccordion.Item
                {
                    ContentContent = options["content"]?.Value<string>("html")?.ConvertHtmlToRenderFragment(),
                    ContentText = options["content"]?.Value<string>("text"),
                    SummaryContent = options["summary"]?.Value<string>("html")?.ConvertHtmlToRenderFragment(),
                    SummaryText = options["summary"]?.Value<string>("text"),
                    HeaderContent = options["heading"]?.Value<string>("html")?.ConvertHtmlToRenderFragment(),
                    HeaderText = options["heading"]?.Value<string>("text"),
                    Expanded = options.Value<bool>("expanded")
                };
            };

            foreach (var item in items)
            {
                if (item is JObject jObjectItem)
                {
                    yield return GetItem(jObjectItem);
                }
                else
                {
                    yield return new GDSAccordion.Item
                    {
                        IsFalsey = true
                    };
                }
            }
        }
    }
}