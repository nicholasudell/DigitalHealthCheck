using System.Collections.Generic;
using System.Linq;
using Bunit;
using DigitalHealthCheckWeb.Components.GDS;
using Newtonsoft.Json.Linq;

namespace BlazorComponentTests
{
    public class SummaryListFactory : ComponentFactory<GDSSummaryList>
    {
        public static GDSSummaryList.Options GetSummaryList(JObject options)
        {
            if (options is null)
            {
                return null;
            }

            return new GDSSummaryList.Options
            {
                Rows = GetRows((JArray)options["rows"]).ToList(),
                Classes = options.Value<string>("classes")
            };
        }

        protected override ComponentParameterCollectionBuilder<GDSSummaryList> AssignParameters(
                    ComponentParameterCollectionBuilder<GDSSummaryList> parameters,
            JObject options)
        {
            var summaryList = GetSummaryList(options);

            parameters.Add(x => x.Classes, summaryList.Classes);
            parameters.Add(x => x.Rows, summaryList.Rows);

            return parameters;
        }

        private static IEnumerable<GDSSummaryList.Row> GetRows(JArray items)
        {
            if (items is null)
            {
                yield break;
            }

            GDSSummaryList.Row.Cell GetCell(JObject options)
            {
                var html = options.Value<string>("html");
                var content = !string.IsNullOrEmpty(html) ? html.ConvertHtmlToRenderFragment() : null;

                return new GDSSummaryList.Row.Cell
                {
                    Classes = options.Value<string>("classes"),
                    Content = content,
                    Text = options.Value<string>("text")
                };
            };

            GDSSummaryList.Row.Item GetItem(JObject options)
            {
                var html = options.Value<string>("html");
                var content = !string.IsNullOrEmpty(html) ? html.ConvertHtmlToRenderFragment() : null;

                var attributes = (JObject)options["attributes"];

                IDictionary<string, object> inputAttributes = new Dictionary<string, object>();

                if (attributes != null)
                {
                    foreach (var attribute in attributes)
                    {
                        inputAttributes.Add(attribute.Key, attribute.Value.Value<string>());
                    }
                }

                return new GDSSummaryList.Row.Item
                {
                    Href = options.Value<string>("href"),
                    Text = options.Value<string>("text"),
                    Content = content,
                    VisuallyHiddenText = options.Value<string>("visuallyHiddenText"),
                    Classes = options.Value<string>("classes"),
                    Attributes = inputAttributes
                };
            }

            IEnumerable<GDSSummaryList.Row.Item> GetItems(JArray items)
            {
                if (items is null)
                {
                    yield break;
                }

                foreach (var item in items)
                {
                    if (item is JObject jObjectItem)
                    {
                        yield return GetItem(jObjectItem);
                    }
                }
            }

            GDSSummaryList.Row GetRow(JObject options) => new()
            {
                Value = GetCell((JObject)options["value"]),
                Key = GetCell((JObject)options["key"]),
                ActionClasses = options["actions"]?.Value<string>("classes"),
                ActionItems = GetItems((JArray)options["actions"]?["items"]).ToList(),
                Classes = options.Value<string>("classes")
            };

            foreach (var item in items)
            {
                if (item is JObject jObjectItem)
                {
                    yield return GetRow(jObjectItem);
                }
            }
        }
    }
}