using Bunit;
using DigitalHealthCheckWeb.Components.GDS;
using Newtonsoft.Json.Linq;

namespace BlazorComponentTests
{
    public class ErrorMessageFactory : ComponentFactory<GDSErrorMessage>
    {
        public static GDSErrorMessage.Options GetErrorMessage(JObject options)
        {
            if (options == null)
            {
                return null;
            }

            return new GDSErrorMessage.Options
            {
                Text = options.Value<string>("text"),
                Classes = options.Value<string>("classes"),
                Id = options.Value<string>("id"),
                VisuallyHiddenText = options.Value<string>("visuallyHiddenText"),
                Attributes = options["attributes"].ConvertToAttributes(),
                Content = options.Value<string>("html").ConvertHtmlToRenderFragment()
            };
        }

        protected override ComponentParameterCollectionBuilder<GDSErrorMessage> AssignParameters
                (
            ComponentParameterCollectionBuilder<GDSErrorMessage> parameters,
            JObject options
        )
        {
            var errorMessage = GetErrorMessage(options);

            parameters.Add(x => x.Text, errorMessage.Text);
            parameters.Add(x => x.Classes, errorMessage.Classes);

            //Horrible boxing and unboxing because attributes has to have object values.
            object id = errorMessage.Id;

            if (string.IsNullOrEmpty((string)id))
            {
                errorMessage.Attributes?.TryGetValue("id", out id);
            }

            parameters.Add(x => x.Id, errorMessage.Id ?? (string)id);
            parameters.Add(x => x.VisuallyHiddenText, errorMessage.VisuallyHiddenText);

            var html = options.Value<string>("html");

            if (!string.IsNullOrEmpty(html))
            {
                parameters.AddChildContent(html);
            }

            return parameters;
        }
    }
}