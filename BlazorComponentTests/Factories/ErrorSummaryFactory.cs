using System.Collections.Generic;
using System.Linq;
using Bunit;
using DigitalHealthCheckWeb.Components.GDS;
using Newtonsoft.Json.Linq;

namespace BlazorComponentTests
{
    public class ErrorSummaryFactory : ComponentFactory<GDSErrorSummary>
    {
        public static GDSErrorSummary.Options GetErrorSummary(JObject options)
        {
            if (options is null)
            {
                return null;
            }

            return new GDSErrorSummary.Options
            {
                Classes = options.Value<string>("classes"),
                Title = new Content
                {
                    Body = options.Value<string>("titleHtml")?.ConvertHtmlToRenderFragment(),
                    Text = options.Value<string>("titleText")
                },
                Description = new Content
                {
                    Body = options.Value<string>("descriptionHtml")?.ConvertHtmlToRenderFragment(),
                    Text = options.Value<string>("descriptionText")
                },
                ErrorList = GetItems((JArray)options["errorList"]).ToList()
            };
        }

        protected override ComponentParameterCollectionBuilder<GDSErrorSummary> AssignParameters
                (
            ComponentParameterCollectionBuilder<GDSErrorSummary> parameters,
            JObject options
        )
        {
            var errorSummary = GetErrorSummary(options);

            parameters.Add(x => x.Classes, errorSummary.Classes);
            parameters.Add(x => x.Description, errorSummary.Description);
            parameters.Add(x => x.ErrorList, errorSummary.ErrorList);
            parameters.Add(x => x.Title, errorSummary.Title);

            return parameters;
        }

        private static IEnumerable<GDSErrorSummary.Item> GetItems(JArray items)
        {
            if (items is null)
            {
                yield break;
            }

            static GDSErrorSummary.Item GetItem(JObject options)
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

                return new GDSErrorSummary.Item
                {
                    Content = new Content
                    {
                        Body = options.Value<string>("html")?.ConvertHtmlToRenderFragment(),
                        Text = options.Value<string>("text")
                    },
                    Href = options.Value<string>("href"),
                    Attributes = options["attributes"].ConvertToAttributes()
                };
            };

            foreach (var item in items)
            {
                yield return GetItem((JObject)item);
            }
        }
    }
}