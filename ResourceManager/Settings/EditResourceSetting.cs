using Spectre.Console.Cli;

namespace ResourceManager.Settings
{
    public class EditResourceSetting : CommandSettings
    {
        [CommandArgument(0, "<ISOLangCode>")]
        public string LangCode { get; set; } = null!;

        [CommandArgument(1, "<ResourceID>")]
        public string ResourceID { get; set; } = null!;

        [CommandArgument(2, "<Text>")]
        public string Text { get; set; } = null!;
    }
}
