using ResourceManager.Attributes;
using Spectre.Console.Cli;

namespace ResourceManager.Settings
{    
    public class PortToResxSetting : CommandSettings
    {
        [CommandArgument(0, "<AbsoluteDestPath>")]
        public string AbsoluteDestPath { get; set; } = null!;

        [CommandArgument(1, "<NeutralCulture>")]
        public string NeutralCulture { get; set; } = null!;
    }
}
