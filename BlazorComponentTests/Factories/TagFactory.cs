using Bunit;
using DigitalHealthCheckWeb.Components.GDS;
using Newtonsoft.Json.Linq;

namespace BlazorComponentTests
{
    public class TagFactory : ComponentFactory<GDSTag>
    {
        public static GDSTag.Options GetTag(JObject options)
        {
            if (options is null)
            {
                return null;
            }

            return new GDSTag.Options
            {
                Attributes = options["attributes"].ConvertToAttributes(),
                Text = options.Value<string>("text"),
                Classes = options.Value<string>("classes")
            };
        }

        protected override ComponentParameterCollectionBuilder<GDSTag> AssignParameters(ComponentParameterCollectionBuilder<GDSTag> parameters, JObject options)
        {
            var tag = GetTag(options);

            parameters.Add(x => x.Text, tag.Text);
            parameters.Add(x => x.Classes, tag.Classes);

            var html = options.Value<string>("html");

            if (!string.IsNullOrEmpty(html))
            {
                parameters.AddChildContent(html);
            }

            return parameters;
        }
    }
}