using ResourceManager.Attributes;
using Spectre.Console.Cli;

namespace ResourceManager.Settings
{    
    public class PortToResxSetting : CommandSettings
    {
        [CommandArgument(0, "<AbsoluteDestPath>")]
        public string AbsoluteDestPath { get; set; } = null!;
    }
}
