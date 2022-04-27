using Bunit;
using DigitalHealthCheckWeb.Components.GDS;
using Newtonsoft.Json.Linq;

namespace BlazorComponentTests
{

    public class ButtonFactory : ComponentFactory<GDSButton>
    {
        public static GDSButton.Options GetButton(JObject options)
        {
            if (options is null)
            {
                return null;
            }

            return new GDSButton.Options
            {
                Attributes = options["attributes"].ConvertToAttributes(),
                Text = options.Value<string>("text"),
                Name = options.Value<string>("name"),
                Classes = options.Value<string>("classes"),
                Disabled = options.Value<bool>("disabled"),
                HRef = options.Value<string>("href"),
                Value = options.Value<string>("value"),
                IsStartButton = options.Value<bool?>("isStartButton") ?? false,
                PreventDoubleClick = options.Value<bool?>("preventDoubleClick") ?? false,
                Element = options.Value<string>("element").ConvertToNullableEnumType<GDSButton.ButtonType>(),
                Type = options.Value<string>("type").ConvertToNullableEnumType<GDSButton.InputType>(),
            };
        }

        protected override ComponentParameterCollectionBuilder<GDSButton> AssignParameters
                (
            ComponentParameterCollectionBuilder<GDSButton> parameters,
            JObject options
        )
        {
            var button = GetButton(options);

            parameters.Add(x => x.Text, button.Text);
            parameters.Add(x => x.Name, button.Name);
            parameters.Add(x => x.Classes, button.Classes);
            parameters.Add(x => x.Disabled, button.Disabled);
            parameters.Add(x => x.HRef, button.HRef);
            parameters.Add(x => x.Value, button.Value);
            parameters.Add(x => x.PreventDoubleClick, button.PreventDoubleClick);
            parameters.Add(x => x.IsStartButton, button.IsStartButton);
            parameters.Add(x => x.Element, button.Element);
            parameters.Add(x => x.Type, button.Type);

            var html = options.Value<string>("html");

            if (!string.IsNullOrEmpty(html))
            {
                parameters.AddChildContent(html);
            }

            return parameters;
        }
    }
}