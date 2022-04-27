using Bunit;
using DigitalHealthCheckWeb.Components.GDS;
using Newtonsoft.Json.Linq;

namespace BlazorComponentTests
{
    public class DetailsFactory : ComponentFactory<GDSDetails>
    {
        public static GDSDetails.Options GetDetails(JObject options)
        {
            if (options == null)
            {
                return null;
            }

            return new GDSDetails.Options
            {
                Attributes = options["attributes"].ConvertToAttributes(),
                Classes = options.Value<string>("classes"),
                Id = options.Value<string>("id"),
                Open = options.Value<bool?>("open") ?? false,
                Text = options.Value<string>("text"),
                SummaryContent = options.Value<string>("summaryHtml").ConvertHtmlToRenderFragment(),
                SummaryText = options.Value<string>("summaryText")
            };
        }

        protected override ComponentParameterCollectionBuilder<GDSDetails> AssignParameters
                (
            ComponentParameterCollectionBuilder<GDSDetails> parameters,
            JObject options
        )
        {
            var details = GetDetails(options);

            parameters.Add(x => x.Classes, details.Classes);
            parameters.Add(x => x.Id, details.Id);
            parameters.Add(x => x.Open, details.Open);
            parameters.Add(x => x.SummaryContent, details.SummaryContent);
            parameters.Add(x => x.SummaryText, details.SummaryText);
            parameters.Add(x => x.Text, details.Text);

            var html = options.Value<string>("html");

            if (!string.IsNullOrEmpty(html))
            {
                parameters.AddChildContent(html);
            }

            return parameters;
        }
    }
}