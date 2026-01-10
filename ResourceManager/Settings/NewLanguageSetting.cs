using Spectre.Console.Cli;

namespace ResourceManager.Settings
{
    public class NewLanguageSetting : CommandSettings
    {
        [CommandArgument(0, "<ISOLangCode>")]
        public string LandCode { get; set; } = null!;
    }
}
