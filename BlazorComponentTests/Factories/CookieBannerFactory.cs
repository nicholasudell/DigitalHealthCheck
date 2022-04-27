using System.Collections.Generic;
using System.Linq;
using Bunit;
using DigitalHealthCheckWeb.Components.GDS;
using Newtonsoft.Json.Linq;

namespace BlazorComponentTests
{
    public class CookieBannerFactory : ComponentFactory<GDSCookieBanner>
    {
        public static GDSCookieBanner.Options GetCookieBanner(JObject options)
        {
            if (options is null)
            {
                return null;
            }

            return new GDSCookieBanner.Options
            {
                Attributes = options["attributes"].ConvertToAttributes(),
                AriaLabel = options.Value<string>("ariaLabel"),
                Hidden = options.Value<bool>("hidden"),
                Messages = GetMessages((JArray)options["messages"]).ToList(),
                Classes = options.Value<string>("classes")

            };
        }

        private static IEnumerable<GDSCookieBanner.Message> GetMessages(JArray items)
        {
            if (items is null)
            {
                yield break;
            }

            static GDSCookieBanner.Message GetMessage(JObject options)
            {
                var attributes = (JObject)options["attributes"];

                IDictionary<string, object> inputAttributes = new Dictionary<string, object>();

                if (attributes != null)
                {
                    foreach (var attribute in attributes)
                    {
                        inputAttributes.Add(attribute.Key, attribute.Value.Value<string>());
                    }
                }

                return new GDSCookieBanner.Message
                {
                    Actions = GetActions((JArray)options["actions"]).ToList(),
                    Classes = options.Value<string>("classes"),
                    Hidden = options.Value<bool>("hidden"),
                    HeadingContent = options.Value<string>("headingHtml")?.ConvertHtmlToRenderFragment(),
                    HeadingText = options.Value<string>("headingText"),
                    Content = options.Value<string>("html")?.ConvertHtmlToRenderFragment(),
                    Text = options.Value<string>("text"),
                    Role = options.Value<string>("role"),
                    InputAttributes = inputAttributes
                };
            };

            foreach (var item in items)
            {
                if (item is JObject jObjectItem)
                {
                    yield return GetMessage(jObjectItem);
                }
            }
        }

        private static IEnumerable<GDSCookieBanner.Action> GetActions(JArray items)
        {
            if (items is null)
            {
                yield break;
            }

            static GDSCookieBanner.Action GetAction(JObject options)
            {
                var attributes = (JObject)options["attributes"];

                IDictionary<string, object> inputAttributes = new Dictionary<string, object>();

                if (attributes != null)
                {
                    foreach (var attribute in attributes)
                    {
                        inputAttributes.Add(attribute.Key, attribute.Value.Value<string>());
                    }
                }

                return new GDSCookieBanner.Action
                {
                    Classes = options.Value<string>("classes"),
                    Text = options.Value<string>("text"),
                    Name = options.Value<string>("name"),
                    Href = options.Value<string>("href"),
                    Value = options.Value<string>("value"),
                    Type = options.Value<string>("type").ConvertToNullableEnumType<GDSButton.InputType>(),
                    InputAttributes = inputAttributes
                };
            };

            foreach (var item in items)
            {
                if (item is JObject jObjectItem)
                {
                    yield return GetAction(jObjectItem);
                }
            }
        }

        protected override ComponentParameterCollectionBuilder<GDSCookieBanner> AssignParameters
                (
            ComponentParameterCollectionBuilder<GDSCookieBanner> parameters,
            JObject options
        )
        {
            var cookieBanner = GetCookieBanner(options);

            parameters.Add(x => x.Hidden, cookieBanner.Hidden);

            if (!string.IsNullOrEmpty(cookieBanner.AriaLabel))
            {
                parameters.Add(x => x.AriaLabel, cookieBanner.AriaLabel);
            }

            parameters.Add(x => x.Messages, cookieBanner.Messages);
            parameters.Add(x => x.Classes, cookieBanner.Classes);

            return parameters;
        }
    }
}