using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace DigitalHealthCheckWeb.Components.Pages
{
    public abstract class FollowUpRiskPageBase : FollowUpPageBase
    {
        [Parameter]
        public IList<Barrier> AllBarriers { get; set; }

        [Parameter]
        public string BarriersError { get; set; }

        [Parameter]
        public string OtherBarrier { get; set; }

        [Parameter]
        public IList<string> SelectedBarriers { get; set; }

        protected IList<Barrier> SelectedBarriersAsBarrierTypes =>
            SelectedBarriers.Select
            (
                x => AllBarriers.SingleOrDefault(y => string.Equals(y.Value, x, StringComparison.OrdinalIgnoreCase))
            )
            .Where(x => x is not null)
            .ToList();
    }
}