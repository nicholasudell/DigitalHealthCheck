using System.Collections.Generic;
using System.Linq;
using Bunit;
using DigitalHealthCheckWeb.Components.GDS;
using Newtonsoft.Json.Linq;

namespace BlazorComponentTests
{
    public class DateInputFactory : ComponentFactory<GDSDateInput>
    {
        public static GDSDateInput.Options GetDateInput(JObject options)
        {
            if (options is null)
            {
                return null;
            }

            return new GDSDateInput.Options
            {
                Name = options.Value<string>("name"),
                Classes = options.Value<string>("classes"),
                Hint = HintFactory.GetHint((JObject)options["hint"]),
                Items = (JArray)options["items"] is null ? null : GetItems((JArray)options["items"]).ToList(),
                ErrorMessage = ErrorMessageFactory.GetErrorMessage((JObject)options["errorMessage"]),
                FieldSet = FieldSetFactory.GetFieldSet((JObject)options["fieldset"]),
                FormGroup = FormGroupFactory.GetFormGroup((JObject)options["formGroup"]),
                Id = options.Value<string>("id"),
                NamePrefix = options.Value<string>("namePrefix")
            };
        }

        protected override ComponentParameterCollectionBuilder<GDSDateInput> AssignParameters(
                    ComponentParameterCollectionBuilder<GDSDateInput> parameters,
            JObject options)
        {
            var dateInput = GetDateInput(options);

            parameters.Add(x => x.Id, dateInput.Id);
            parameters.Add(x => x.Name, dateInput.Name);
            parameters.Add(x => x.Classes, dateInput.Classes);
            parameters.Add(x => x.FormGroup, dateInput.FormGroup);
            parameters.Add(x => x.Hint, dateInput.Hint);
            parameters.Add(x => x.Items, dateInput.Items);
            parameters.Add(x => x.ErrorMessage, dateInput.ErrorMessage);
            parameters.Add(x => x.FieldSet, dateInput.FieldSet);
            parameters.Add(x => x.NamePrefix, dateInput.NamePrefix);

            return parameters;
        }

        private static IEnumerable<GDSDateInput.Item> GetItems(JArray items)
        {
            if (items is null)
            {
                yield break;
            }

            static GDSDateInput.Item GetItem(JObject options) => new()
            {
                Attributes = options["attributes"]?.ConvertToAttributes(),
                Id = options.Value<string>("id"),
                Label = options.Value<string>("label"),
                Value = options.Value<string>("value"),
                Name = options.Value<string>("name"),
                InputMode = options.Value<string>("inputmode"),
                Pattern = options.Value<string>("pattern"),
                Autocomplete = options.Value<string>("autocomplete"),
                Classes = options.Value<string>("classes")
            };

            foreach (var item in items)
            {
                if (item is JObject jObjectItem)
                {
                    yield return GetItem(jObjectItem);
                }
            }
        }
    }
}