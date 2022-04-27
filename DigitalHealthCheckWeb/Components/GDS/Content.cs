using Microsoft.AspNetCore.Components;

namespace DigitalHealthCheckWeb.Components.GDS
{
    public class Content
    {
        public RenderFragment Body { get; set; }

        public string Text { get; set; }

        public bool UseBody => Body is not null;

        public Content()
        {
        }

        public Content(string text) => Text = text;

        public Content(RenderFragment body) => Body = body;

        public Content(RenderFragment body, string text)
        {
            Body = body;
            Text = text;
        }

        public static explicit operator RenderFragment(Content content) => content.Body;

        public static explicit operator string(Content content) => content.Text;

        public static implicit operator Content(string text) => new(text);

        public static implicit operator Content(RenderFragment renderFragment) => new(renderFragment);
    }
}