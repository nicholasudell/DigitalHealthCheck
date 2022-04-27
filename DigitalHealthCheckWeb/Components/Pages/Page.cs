using System.Collections.Generic;
using System.Linq;
using DigitalHealthCheckWeb.Components.GDS;
using Microsoft.AspNetCore.Components;

namespace DigitalHealthCheckWeb.Components.Pages
{
    public class Page : ComponentBase
    {
        [Parameter]
        public string AntiForgery { get; set; }

        [Parameter]
        public IEnumerable<GDSErrorSummary.Item> ErrorList { get; set; } = new List<GDSErrorSummary.Item>();

        [Parameter]
        public string Id { get; set; }

        protected string IdQueryParam => $"id={Id}";

        protected string BuildQuery(params string[] queryParams)
        {
            var filteredQueryParams = queryParams.Where(x => !string.IsNullOrEmpty(x));

            if (!filteredQueryParams.Any())
            {
                return string.Empty;
            }

            return $"?{string.Join('&', filteredQueryParams.ToArray())}";
        }
    }
}