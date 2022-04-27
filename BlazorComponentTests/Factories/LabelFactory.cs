using Bunit;
using DigitalHealthCheckWeb.Components.GDS;
using Newtonsoft.Json.Linq;

namespace BlazorComponentTests
{
    public class LabelFactory : ComponentFactory<GDSLabel>
    {
        public static GDSLabel.Options GetLabel(JObject options)
        {
            if (options == null)
            {
                return null;
            }

            return new GDSLabel.Options()
            {
                Attributes = options["attributes"].ConvertToAttributes(),
                Classes = options.Value<string>("classes"),
                Text = options.Value<string>("text"),
                IsPageHeading = options.Value<bool?>("isPageHeading") ?? false,
                Content = options.Value<string>("html").ConvertHtmlToRenderFragment(),
                For = options.Value<string>("for")
            };
        }

        protected override ComponentParameterCollectionBuilder<GDSLabel> AssignParameters
                (
            ComponentParameterCollectionBuilder<GDSLabel> parameters,
            JObject options
        )
        {
            var label = GetLabel(options);

            parameters.Add(x => x.Text, label.Text);
            parameters.Add(x => x.Classes, label.Classes);
            parameters.Add(x => x.For, label.For);
            parameters.Add(x => x.IsPageHeading, label.IsPageHeading);

            var html = options.Value<string>("html");

            if (!string.IsNullOrEmpty(html))
            {
                parameters.AddChildContent(html);
            }

            return parameters;
        }
    }
}