using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace DigitalHealthCheckWeb.Components.Pages
{
    public abstract class FollowUpNumbersPageBase : FollowUpPageBase
    {
        [Parameter]
        public IList<Barrier> AllBarriers { get; set; }

        [Parameter]
        public string BarriersError { get; set; }

        [Parameter]
        public string BaseUrl { get; set; }

        [Parameter]
        public string OtherBarrier { get; set; }

        [Parameter]
        public IList<string> SelectedBarriers { get; set; }

        [Parameter]
        public string SelectedOption { get; set; }

        [Parameter]
        public string SelectedOptionError { get; set; }

        protected IList<Barrier> SelectedBarriersAsBarrierTypes =>
            SelectedBarriers.Select
            (
                x => AllBarriers.SingleOrDefault(y => string.Equals(y.Value, x, StringComparison.OrdinalIgnoreCase))
            )
            .Where(x => x is not null)
            .ToList();
    }
}