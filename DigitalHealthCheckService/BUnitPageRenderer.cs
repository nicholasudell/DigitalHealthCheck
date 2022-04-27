using System;
using Bunit;
using Microsoft.AspNetCore.Components;

namespace DigitalHealthCheckService
{
    public class BUnitPageRenderer
    {
        public string RenderHtml<TPage>(Func<ComponentParameterCollectionBuilder<TPage>, ComponentParameterCollectionBuilder<TPage>> componentParameterCollectionBuilder)
            where TPage : IComponent
        {
            using var context = new Bunit.TestContext();

            return context.RenderComponent<TPage>(p => componentParameterCollectionBuilder(p)).Markup;
        }
    }
}
