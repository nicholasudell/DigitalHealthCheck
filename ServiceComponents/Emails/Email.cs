using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using DigitalHealthCheckEF;
namespace ServiceComponents.Emails
{
    public class PatientEmail : ComponentBase
    {
        [Parameter]
        public HealthCheck Check { get; set; }

        [Parameter]
        public string BaseUrl { get; set; }

        protected string BuildQuery(params string[] queryParams)
        {
            var filteredQueryParams = queryParams.Where(x => !string.IsNullOrEmpty(x));

            if (!filteredQueryParams.Any())
            {
                return string.Empty;
            }

            return $"?{string.Join('&', filteredQueryParams.ToArray())}";
        }

        protected string Url(string page)
        {
            if(page == "CheckYourAnswers")
            {
                return $"{BaseUrl}/{page}{BuildQuery(IdQueryParam)}";
            }

            return $"{BaseUrl}/{page}{BuildQuery(IdQueryParam, $"next=CheckYourAnswers")}";
        }

        protected string Id => Check.Id.ToString();

        protected string IdQueryParam => $"id={Id}";
    }
}