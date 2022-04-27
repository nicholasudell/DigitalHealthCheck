namespace GenerateLinks
{
    public static class ConsoleExtensions
    {
        public static string ArgOrDefault(this string[] args, int index, string @default) =>
            args.Length > index ? args[index] : @default;
    }
}