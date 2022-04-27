using Bunit;
using Newtonsoft.Json.Linq;
using TestContext = Bunit.TestContext;

namespace BlazorComponentTests
{
    public abstract class ComponentFactory<T> : IComponentFactory
        where T : Microsoft.AspNetCore.Components.IComponent
    {
        public virtual IRenderedComponent<T> CreateComponent(TestContext context, JObject options) =>
            context.RenderComponent<T>(p =>
            {
                var attributes = (JObject)options["attributes"];

                if (attributes != null)
                {
                    p = AssignAttributes(p, attributes);
                }

                p = AssignParameters(p, options);
            });

        IRenderedFragment IComponentFactory.CreateComponent(TestContext context, JObject fixture)
            => CreateComponent(context, fixture);

        protected ComponentParameterCollectionBuilder<T> AssignAttributes(ComponentParameterCollectionBuilder<T> parameters, JObject attributes)
        {
            foreach (var attribute in attributes)
            {
                parameters.AddUnmatched(attribute.Key, attribute.Value.Value<string>());
            }

            return parameters;
        }

        protected abstract ComponentParameterCollectionBuilder<T> AssignParameters(ComponentParameterCollectionBuilder<T> parameters, JObject options);
    }
}