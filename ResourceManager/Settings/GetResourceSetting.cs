using Spectre.Console.Cli;

namespace ResourceManager.Settings
{
    public class GetResourceSetting : CommandSettings
    {
        [CommandArgument(0, "<ResourceID>")]
        public string ResourceID { get; set; } = null!;
    }
}
