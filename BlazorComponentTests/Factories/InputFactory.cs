using Bunit;
using DigitalHealthCheckWeb.Components.GDS;
using Newtonsoft.Json.Linq;

namespace BlazorComponentTests
{
    public class InputFactory : ComponentFactory<GDSInput>
    {
        public static GDSInput.AffixOptions GetAffix(JObject options)
        {
            if (options == null)
            {
                return null;
            }

            return new GDSInput.AffixOptions
            {
                Attributes = options["attributes"].ConvertToAttributes(),
                Classes = options.Value<string>("classes"),
                Content = options.Value<string>("html").ConvertHtmlToRenderFragment(),
                Text = options.Value<string>("text")
            };
        }

        public static GDSInput.Options GetInput(JObject options) => new()
        {
            Name = options.Value<string>("name"),
            Classes = options.Value<string>("classes"),
            Id = options.Value<string>("id"),
            Autocomplete = options.Value<string>("autocomplete"),
            DescribedBy = options.Value<string>("describedBy"),
            Type = options.Value<string>("type"),
            InputMode = options.Value<string>("inputmode"),
            Pattern = options.Value<string>("pattern"),
            Spellcheck = options.Value<bool?>("spellcheck"),
            Value = options.Value<string>("value"),
            FormGroupClasses = options["formGroup"]?.Value<string>("classes"),
            ErrorMessage = ErrorMessageFactory.GetErrorMessage((JObject)options["errorMessage"]),
            Label = LabelFactory.GetLabel((JObject)options["label"]),
            Hint = HintFactory.GetHint((JObject)options["hint"]),
            InputAttributes = options["attributes"].ConvertToAttributes(),
            Prefix = GetAffix((JObject)options["prefix"]),
            Suffix = GetAffix((JObject)options["suffix"])
        };

        protected override ComponentParameterCollectionBuilder<GDSInput> AssignParameters
        (
            ComponentParameterCollectionBuilder<GDSInput> parameters,
            JObject options
        )
        {
            var input = GetInput(options);

            parameters.Add(x => x.Name, input.Name);
            parameters.Add(x => x.Classes, input.Classes);
            parameters.Add(x => x.Id, input.Id);
            parameters.Add(x => x.Autocomplete, input.Autocomplete);
            parameters.Add(x => x.DescribedBy, input.DescribedBy);
            parameters.Add(x => x.Type, input.Type);
            parameters.Add(x => x.InputMode, input.InputMode);
            parameters.Add(x => x.Pattern, input.Pattern);
            parameters.Add(x => x.Spellcheck, input.Spellcheck);
            parameters.Add(x => x.Value, input.Value);
            parameters.Add(x => x.Prefix, input.Prefix);
            parameters.Add(x => x.Suffix, input.Suffix);
            parameters.Add(x => x.Label, input.Label);
            parameters.Add(x => x.Hint, input.Hint);
            parameters.Add(x => x.ErrorMessage, input.ErrorMessage);
            parameters.Add(x => x.FormGroupClasses, input.FormGroupClasses);

            return parameters;
        }
    }
}