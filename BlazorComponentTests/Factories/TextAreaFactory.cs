using Bunit;
using DigitalHealthCheckWeb.Components.GDS;
using Newtonsoft.Json.Linq;

namespace BlazorComponentTests
{
    public class CharacterCountFactory : ComponentFactory<GDSCharacterCount>
    {
        public static GDSCharacterCount.Options GetInput(JObject options) => new GDSCharacterCount.Options
        {
            Id = options.Value<string>("id"),
            MaxWords = options.Value<int?>("maxwords"),
            MaxLength = options.Value<int?>("maxlength"),
            Threshold = options.Value<int?>("threshold"),
            CountMessageClasses = options["countMessage"]?.Value<string>("classes"),
            Hint = HintFactory.GetHint((JObject)options["hint"]),
            TextArea = TextAreaFactory.GetInput(options)
        };

        protected override ComponentParameterCollectionBuilder<GDSCharacterCount> AssignParameters
               (
           ComponentParameterCollectionBuilder<GDSCharacterCount> parameters,
           JObject options
       )
        {
            var input = GetInput(options);

            parameters.Add(x => x.Id, input.Id);
            parameters.Add(x => x.Hint, input.Hint);
            parameters.Add(x => x.TextArea, input.TextArea);
            parameters.Add(x => x.CountMessageClasses, input.CountMessageClasses);
            parameters.Add(x => x.MaxLength, input.MaxLength);
            parameters.Add(x => x.MaxWords, input.MaxWords);
            parameters.Add(x => x.Threshold, input.Threshold);
            return parameters;
        }
    }

    public class TextAreaFactory : ComponentFactory<GDSTextArea>
    {
        public static GDSTextArea.Options GetInput(JObject options) => new GDSTextArea.Options
        {
            Name = options.Value<string>("name"),
            Classes = options.Value<string>("classes"),
            Id = options.Value<string>("id"),
            Autocomplete = options.Value<string>("autocomplete"),
            DescribedBy = options.Value<string>("describedBy"),
            Spellcheck = options.Value<bool?>("spellcheck"),
            Value = options.Value<string>("value"),
            FormGroup = FormGroupFactory.GetFormGroup((JObject)options["formGroup"]),
            ErrorMessage = ErrorMessageFactory.GetErrorMessage((JObject)options["errorMessage"]),
            Label = LabelFactory.GetLabel((JObject)options["label"]),
            Hint = HintFactory.GetHint((JObject)options["hint"]),
            Rows = options.Value<int?>("rows")
        };

        protected override ComponentParameterCollectionBuilder<GDSTextArea> AssignParameters
               (
           ComponentParameterCollectionBuilder<GDSTextArea> parameters,
           JObject options
       )
        {
            var input = GetInput(options);

            parameters.Add(x => x.Name, input.Name);
            parameters.Add(x => x.Classes, input.Classes);
            parameters.Add(x => x.Id, input.Id);
            parameters.Add(x => x.Autocomplete, input.Autocomplete);
            parameters.Add(x => x.DescribedBy, input.DescribedBy);
            parameters.Add(x => x.Spellcheck, input.Spellcheck);
            parameters.Add(x => x.Value, input.Value);
            parameters.Add(x => x.Label, input.Label);
            parameters.Add(x => x.Hint, input.Hint);
            parameters.Add(x => x.ErrorMessage, input.ErrorMessage);
            parameters.Add(x => x.FormGroup, input.FormGroup);
            parameters.Add(x => x.Rows, input.Rows);

            return parameters;
        }
    }
}