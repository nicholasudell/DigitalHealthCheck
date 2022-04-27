using Bunit;
using DigitalHealthCheckWeb.Components.GDS;
using Newtonsoft.Json.Linq;

namespace BlazorComponentTests
{
    public class FieldSetFactory : ComponentFactory<GDSFieldSet>
    {
        public static GDSFieldSet.Options GetFieldSet(JObject options)
        {
            if (options == null)
            {
                return null;
            }

            return new GDSFieldSet.Options
            {
                Attributes = options["attributes"].ConvertToAttributes(),
                Classes = options.Value<string>("classes"),
                DescribedBy = options.Value<string>("describedBy"),
                Role = options.Value<string>("role"),
                Legend = GetLegend((JObject)options["legend"])
            };
        }

        protected override ComponentParameterCollectionBuilder<GDSFieldSet> AssignParameters
                (
            ComponentParameterCollectionBuilder<GDSFieldSet> parameters,
            JObject options
        )
        {
            var fieldSet = GetFieldSet(options);

            parameters.Add(x => x.Classes, fieldSet.Classes);
            parameters.Add(x => x.DescribedBy, fieldSet.DescribedBy);
            parameters.Add(x => x.Role, fieldSet.Role);

            var html = options.Value<string>("html");

            if (!string.IsNullOrEmpty(html))
            {
                parameters.AddChildContent(html);
            }

            parameters.Add(x => x.Legend, fieldSet.Legend);

            return parameters;
        }

        private static GDSFieldSet.LegendOptions GetLegend(JObject options)
        {
            if (options == null)
            {
                return null;
            }

            return new GDSFieldSet.LegendOptions
            {
                Classes = options.Value<string>("classes"),
                IsPageHeading = options.Value<bool?>("isPageHeading") ?? false,
                Content = options.Value<string>("html").ConvertHtmlToRenderFragment(),
                Text = options.Value<string>("text")
            };
        }
    }
}