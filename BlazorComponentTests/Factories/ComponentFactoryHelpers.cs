using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json.Linq;

namespace BlazorComponentTests
{
    public static class ComponentFactoryHelpers
    {
        public static RenderFragment ConvertHtmlToRenderFragment(this string html) =>
            string.IsNullOrEmpty(html) ?
                null :
                builder =>
                {
                    builder.AddMarkupContent(0, html);
                };

        public static IDictionary<string, object> ConvertToAttributes(this JToken attributeOptions)
        {
            var attributes = new Dictionary<string, object>();

            if (attributeOptions != null)
            {
                foreach (var attribute in (JObject)attributeOptions)
                {
                    attributes.Add(attribute.Key, attribute.Value.Value<string>());
                }
            }

            return attributes;
        }

        public static Nullable<T> ConvertToNullableEnumType<T>(this string value) where T : struct =>
                            (Nullable<T>)(string.IsNullOrEmpty(value) ? null : Enum.Parse(typeof(T), value, true));
    }
}