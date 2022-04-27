using System;
using System.Collections.Generic;
using System.Linq;
using DigitalHealthCheckWeb.Components.GDS;

namespace DigitalHealthCheckWeb.Components
{
    public static class RazorComponentExtensions
    {
        public static IList<GDSCheckboxes.Item> AsCheckboxComponents<T>(this IEnumerable<T> itemSource, Func<T, string> valueSelector, Func<T, string> textSelector, Func<T, bool> isSelected)
            => itemSource.Select(x => new GDSCheckboxes.Item
            {
                Text = textSelector(x),
                Value = valueSelector(x),
                Checked = isSelected(x)
            }).ToList();

        public static GDSErrorMessage.Options AsErrorMessageComponent(this string errorMessage) =>
            string.IsNullOrEmpty(errorMessage) ? null :
                new GDSErrorMessage.Options
                {
                    Text = errorMessage
                };

        public static IList<GDSRadios.Item> AsRadioComponents<T>(this IEnumerable<T> itemSource, Func<T, string> valueSelector, Func<T, string> textSelector, string currentValue)
                            => itemSource.Select(x => new GDSRadios.Item
                            {
                                Text = textSelector(x),
                                Value = valueSelector(x),
                                Checked = valueSelector(x) == currentValue
                            }).ToList();

        public static IEnumerable<GDSCheckboxes.Item> WithNoneOption(this IEnumerable<GDSCheckboxes.Item> items, string text, string value = "none")
            => items.Concat(new[]
            {
                new GDSCheckboxes.Item
                {
                     Divider = "or"
                },
                new GDSCheckboxes.Item
                {
                    Behaviour="exclusive",
                    Text=text,
                    Value=value
                }
            });
    }
}