using Bunit;
using Newtonsoft.Json.Linq;
using TestContext = Bunit.TestContext;

namespace BlazorComponentTests
{
    public interface IComponentFactory
    {
        IRenderedFragment CreateComponent(TestContext context, JObject options);
    }
}