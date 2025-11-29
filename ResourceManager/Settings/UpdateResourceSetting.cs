using Spectre.Console.Cli;

namespace ResourceManager.Settings
{
    public class UpdateResourceSetting : CommandSettings
    {
        [CommandArgument(0, "<ResourceID>")]
        public string ResourceID { get; set; } = null!;

        [CommandArgument(1, "<Text>")]
        public string Text { get; set; } = null!;
    }
}
