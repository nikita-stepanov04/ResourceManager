using Spectre.Console.Cli;

namespace ResourceManager.Settings
{
    public class ResourceSettings : CommandSettings
    {
        [CommandArgument(0, "<Text>")]
        public string Text { get; set; } = null!;
    }
}
