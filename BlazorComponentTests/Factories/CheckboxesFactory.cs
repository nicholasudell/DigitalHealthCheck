using System.Collections.Generic;
using System.Linq;
using Bunit;
using DigitalHealthCheckWeb.Components.GDS;
using Newtonsoft.Json.Linq;

namespace BlazorComponentTests
{
    public class CheckboxesFactory : ComponentFactory<GDSCheckboxes>
    {
        public static GDSCheckboxes.Options GetCheckboxes(JObject options)
        {
            if (options is null)
            {
                return null;
            }

            return new GDSCheckboxes.Options
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
                InputAttributes = options["attributes"].ConvertToAttributes(),
                DescribedBy = options.Value<string>("describedBy")
            };
        }

        protected override ComponentParameterCollectionBuilder<GDSCheckboxes> AssignParameters(
                    ComponentParameterCollectionBuilder<GDSCheckboxes> parameters,
            JObject options)
        {
            var checkboxes = GetCheckboxes(options);

            parameters.Add(x => x.Name, checkboxes.Name);
            parameters.Add(x => x.Classes, checkboxes.Classes);
            parameters.Add(x => x.FormGroup, checkboxes.FormGroup);
            parameters.Add(x => x.IdPrefix, checkboxes.IdPrefix);
            parameters.Add(x => x.Label, checkboxes.Label);
            parameters.Add(x => x.Hint, checkboxes.Hint);
            parameters.Add(x => x.Items, checkboxes.Items);
            parameters.Add(x => x.ErrorMessage, checkboxes.ErrorMessage);
            parameters.Add(x => x.FieldSet, checkboxes.FieldSet);
            parameters.Add(x => x.DescribedBy, checkboxes.DescribedBy);

            var html = options.Value<string>("html");

            if (!string.IsNullOrEmpty(html))
            {
                parameters.AddChildContent(html);
            }

            return parameters;
        }

        private static IEnumerable<GDSCheckboxes.Item> GetItems(JArray items)
        {
            if (items is null)
            {
                yield break;
            }

            static GDSCheckboxes.Item GetItem(JObject options)
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

                return new GDSCheckboxes.Item
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
                    Value = options.Value<string>("value"),
                    Behaviour = options.Value<string>("behaviour"),
                    Name = options.Value<string>("name")
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
                    yield return new GDSCheckboxes.Item
                    {
                        IsFalsey = true
                    };
                }
            }
        }
    }
}