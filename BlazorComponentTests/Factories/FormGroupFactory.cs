using DigitalHealthCheckWeb.Components.GDS;
using Newtonsoft.Json.Linq;

namespace BlazorComponentTests
{
    public class FormGroupFactory
    {
        public static GDSFormGroup.Options GetFormGroup(JObject options)
        {
            if (options is null)
            {
                return null;
            }

            return new GDSFormGroup.Options()
            {
                Classes = options.Value<string>("classes")
            };
        }
    }
}