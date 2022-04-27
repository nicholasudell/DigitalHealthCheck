using Bunit;
using DigitalHealthCheckWeb.Components.GDS;
using Newtonsoft.Json.Linq;

namespace BlazorComponentTests
{

    public class PanelFactory : ComponentFactory<GDSPanel>
    {
        public static GDSPanel.Options GetPanel(JObject options)
        {
            if (options is null)
            {
                return null;
            }

            return new GDSPanel.Options
            {
                Text = options.Value<string>("text"),
                Classes = options.Value<string>("classes"),
                HeadingLevel = options.Value<int?>("headingLevel"),
                TitleText = options.Value<string>("titleText"),
                TitleContent = options.Value<string>("titleHtml").ConvertHtmlToRenderFragment(),
            };
        }

        protected override ComponentParameterCollectionBuilder<GDSPanel> AssignParameters(
                    ComponentParameterCollectionBuilder<GDSPanel> parameters,
            JObject options)
        {
            var panel = GetPanel(options);

            parameters.Add(x => x.Classes, panel.Classes);
            parameters.Add(x => x.HeadingLevel, panel.HeadingLevel);
            parameters.Add(x => x.Text, panel.Text);
            parameters.Add(x => x.TitleContent, panel.TitleContent);
            parameters.Add(x => x.TitleText, panel.TitleText);

            var html = options.Value<string>("html");

            if (!string.IsNullOrEmpty(html))
            {
                parameters.AddChildContent(html);
            }

            return parameters;
        }
    }
}