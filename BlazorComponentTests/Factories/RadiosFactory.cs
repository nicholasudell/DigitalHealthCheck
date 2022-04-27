using System.Collections.Generic;
using System.Linq;
using Bunit;
using DigitalHealthCheckWeb.Components.GDS;
using Newtonsoft.Json.Linq;

namespace BlazorComponentTests
{
    public class RadiosFactory : ComponentFactory<GDSRadios>
    {
        public static GDSRadios.Options GetRadios(JObject options)
        {
            if (options is null)
            {
                return null;
            }

            return new GDSRadios.Options
            {
                Name = options.Value<string>("name"),
                Classes = options.Value<string>("classes"),
                Label = LabelFactory.GetLabel((JObject)options["label"]),
                Hint = HintFactory.GetHint((JObject)options["hint"]),
                Items = GetItems((JArray)options["items"]).ToList(),
                ErrorMessage = ErrorMessageFactory.GetErrorMessage((JObject)options["errorMessage"]),
                FieldSet = FieldSetFactory.GetFieldSet((JObject)options["fieldset"]),
                FormGroup = FormGroupFactory.GetFormGroup((JObject)options["formGroup"]),
                IdPrefix = options.Value<string>("idPrefix"),
                Content = options.Value<string>("html").ConvertHtmlToRenderFragment(),
                InputAttributes = options["attributes"].ConvertToAttributes()
            };
        }

        protected override ComponentParameterCollectionBuilder<GDSRadios> AssignParameters
                (
            ComponentParameterCollectionBuilder<GDSRadios> parameters,
            JObject options
        )
        {
            var radios = GetRadios(options);

            parameters.Add(x => x.Name, radios.Name);
            parameters.Add(x => x.Classes, radios.Classes);
            parameters.Add(x => x.FormGroup, radios.FormGroup);
            parameters.Add(x => x.IdPrefix, radios.IdPrefix);
            parameters.Add(x => x.Label, radios.Label);
            parameters.Add(x => x.Hint, radios.Hint);
            parameters.Add(x => x.Items, radios.Items);
            parameters.Add(x => x.ErrorMessage, radios.ErrorMessage);
            parameters.Add(x => x.FieldSet, radios.FieldSet);

            var html = options.Value<string>("html");

            if (!string.IsNullOrEmpty(html))
            {
                parameters.AddChildContent(html);
            }

            return parameters;
        }

        private static IEnumerable<GDSRadios.Item> GetItems(JArray items)
        {
            if (items is null)
            {
                yield break;
            }

            static GDSRadios.Item GetItem(JObject options)
            {
                var conditionalContentString = options["conditional"]?.Value<string>("html");

                // Kludge to fix a test for options intended to be invalid that would be valid as a
                // RenderFragment. The test assumes that "false" is equivalent to absence of data,
                // even though the input here is a RenderFragment, and a fragment containing the
                // string "False" is neither null nor invalid content.

                if (conditionalContentString == "False")
                {
                    conditionalContentString = null;
                }

                return new GDSRadios.Item
                {
                    Attributes = options["attributes"]?.ConvertToAttributes(),
                    Checked = options.Value<bool?>("checked") ?? false,
                    ConditionalContent = conditionalContentString?.ConvertHtmlToRenderFragment(),
                    Content = options.Value<string>("html")?.ConvertHtmlToRenderFragment(),
                    Disabled = options.Value<bool?>("disabled") ?? false,
                    Divider = options.Value<string>("divider"),
                    Hint = HintFactory.GetHint((JObject)options["hint"]),
                    Id = options.Value<string>("id"),
                    Label = LabelFactory.GetLabel((JObject)options["label"]),
                    Text = options.Value<string>("text"),
                    Value = options.Value<string>("value")
                };
            };

            foreach (var item in items)
            {
                if (item is JObject jObjectItem)
                {
                    yield return GetItem(jObjectItem);
                }
                else
                {
                    yield return new GDSRadios.Item
                    {
                        IsFalsey = true
                    };
                }
            }
        }
    }
}