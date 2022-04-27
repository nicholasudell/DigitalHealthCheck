using Bunit;
using DigitalHealthCheckWeb.Components.GDS;
using Newtonsoft.Json.Linq;

namespace BlazorComponentTests
{
    public class HintFactory : ComponentFactory<GDSHint>
    {
        /// <summary>
        /// Gets the hint.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="options">The hint options.</param>
        /// <returns></returns>
        public static GDSHint.Options GetHint(JObject options)
        {
            if (options == null)
            {
                return null;
            }

            return new GDSHint.Options
            {
                Attributes = options["attributes"].ConvertToAttributes(),
                Classes = options.Value<string>("classes"),
                Content = options.Value<string>("html").ConvertHtmlToRenderFragment(),
                Id = options.Value<string>("id"),
                Text = options.Value<string>("text")
            };
        }

        protected override ComponentParameterCollectionBuilder<GDSHint> AssignParameters
                (
            ComponentParameterCollectionBuilder<GDSHint> parameters,
            JObject options
        )
        {
            var hint = GetHint(options);

            parameters.Add(x => x.Text, hint.Text);
            parameters.Add(x => x.Classes, hint.Classes);
            parameters.Add(x => x.Id, hint.Id);

            var html = options.Value<string>("html");

            if (!string.IsNullOrEmpty(html))
            {
                parameters.AddChildContent(html);
            }

            return parameters;
        }
    }
}