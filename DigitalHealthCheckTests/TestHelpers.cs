using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DigitalHealthCheckTests
{
    public static class TestHelpers
    {
        public static IDictionary<string, string> AsStringsDictionary(this object source,
            BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance) => source.GetType().GetProperties(bindingAttr).ToDictionary
                (
                    propInfo => propInfo.Name,
                    propInfo => Convert.ToString(propInfo.GetValue(source, null))
                );

        public static string Print
        (
            this object source,
            string separator = "\r\n",
            BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance
        ) => string.Join(separator, GetProperties(source, x => true, bindingAttr)
                 .Select(kvp => $"{kvp.Key}: {kvp.Value}")
                 .ToArray()
             );

        public static string Print
        (
            this object source,
            Func<KeyValuePair<string, string>, bool> filter,
            string separator = "\r\n",
            BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance
        ) => string.Join(separator, GetProperties(source, filter, bindingAttr)
                 .Where(x => !string.IsNullOrEmpty(x.Value) && filter(x))
                 .Select(kvp => $"{kvp.Key}: {kvp.Value}")
                 .ToArray()
             );

        public static string PrintForTestName
        (
            this object source,
            string separator = "_",
            BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance
        ) => string.Join(separator, GetProperties(source, x => true, bindingAttr)
                .Select(kvp => $"{kvp.Key}_{kvp.Value}".Replace('.', '_'))
                .ToArray()
            );

        public static string PrintForTestName
        (
            this object source,
            Func<KeyValuePair<string, string>, bool> filter,
            string separator = "_",
            BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance
        ) => string.Join(separator, GetProperties(source, filter, bindingAttr)
                .Select(kvp => $"{kvp.Key}_{kvp.Value}".Replace('.', '_'))
                .ToArray()
            );

        private static IEnumerable<KeyValuePair<string, string>> GetProperties(
                    object source,
            Func<KeyValuePair<string, string>, bool> filter,
            BindingFlags bindingAttr) =>
            source.AsStringsDictionary(bindingAttr)
                .Where(x => !string.IsNullOrEmpty(x.Value) && filter(x));
    }
}