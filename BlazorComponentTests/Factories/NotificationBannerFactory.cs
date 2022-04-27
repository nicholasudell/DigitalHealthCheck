using Bunit;
using DigitalHealthCheckWeb.Components.GDS;
using Newtonsoft.Json.Linq;

namespace BlazorComponentTests
{
    public class NotificationBannerFactory : ComponentFactory<GDSNotificationBanner>
    {
        public static GDSNotificationBanner.Options GetNotificationBanner(JObject options)
        {
            if (options is null)
            {
                return null;
            }

            return new GDSNotificationBanner.Options
            {
                Text = options.Value<string>("text"),
                Classes = options.Value<string>("classes"),
                TitleHeadingLevel = options.Value<int?>("titleHeadingLevel"),
                TitleText = options.Value<string>("titleText"),
                TitleContent = options.Value<string>("titleHtml").ConvertHtmlToRenderFragment(),
                DisableAutoFocus = options.Value<bool>("disableAutoFocus"),
                 TitleId = options.Value<string>("titleId"),
                  Type = options.Value<string>("type"),
                Role = options.Value<string>("role"),
            };
        }

        protected override ComponentParameterCollectionBuilder<GDSNotificationBanner> AssignParameters(
                    ComponentParameterCollectionBuilder<GDSNotificationBanner> parameters,
            JObject options)
        {
            var NotificationBanner = GetNotificationBanner(options);

            parameters.Add(x => x.Classes, NotificationBanner.Classes);
            parameters.Add(x => x.TitleHeadingLevel, NotificationBanner.TitleHeadingLevel);
            parameters.Add(x => x.Text, NotificationBanner.Text);
            parameters.Add(x => x.TitleId, NotificationBanner.TitleId);
            parameters.Add(x => x.Type, NotificationBanner.Type);
            parameters.Add(x => x.Role, NotificationBanner.Role);
            parameters.Add(x => x.DisableAutoFocus, NotificationBanner.DisableAutoFocus);
            parameters.Add(x => x.TitleContent, NotificationBanner.TitleContent);
            parameters.Add(x => x.TitleText, NotificationBanner.TitleText);

            var html = options.Value<string>("html");

            if (!string.IsNullOrEmpty(html))
            {
                parameters.AddChildContent(html);
            }

            return parameters;
        }
    }
}