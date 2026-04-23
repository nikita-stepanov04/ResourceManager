namespace ResourceManager.Helpers
{
    public static class Colors
    {
        private const string COLOR_TEMPLATE = "[{0}]{1}[/]";

        private const string COLOR_GREEN = "green";
        private const string COLOR_RED = "red";
        private const string COLOR_YELLOW = "yellow";
        private const string COLOR_BLUE = "blue";
        private const string COLOR_FUCHSIA = "fuchsia";

        public static string Green(string text) => string.Format(COLOR_TEMPLATE, COLOR_GREEN, text);
        public static string Red(string text) => string.Format(COLOR_TEMPLATE, COLOR_RED, text);
        public static string Yellow(string text) => string.Format(COLOR_TEMPLATE, COLOR_YELLOW, text);
        public static string Blue(string text) => string.Format(COLOR_TEMPLATE, COLOR_BLUE, text);
        public static string Fuchsia(string text) => string.Format(COLOR_TEMPLATE, COLOR_FUCHSIA, text);
    }
}
