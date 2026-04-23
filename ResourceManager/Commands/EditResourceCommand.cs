using ResourceManager.Attributes;
using ResourceManager.Helpers;
using ResourceManager.Settings;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ResourceManager.Commands
{
    [CommandName("edit", ordering: 4)]
    [CommandDescription("Edit a resource by resource ID and language " +
        "code with a new value, other languages remain unchanged")]
    public class EditResourceCommand : Command<EditResourceSetting>
    {
        public override int Execute(
            CommandContext context, EditResourceSetting settings, CancellationToken cancellationToken)
        {
            var config = Configuration.GetConfig();
            if (config == null)
                return -1;

            var (resourcesDict, languages) = Resources.GetResources(config.ResourcesFolder);
            if (resourcesDict == null)
                return -1;

            if (!resourcesDict.Values.First().Data.ContainsKey(settings.ResourceID))
            {
                AnsiConsole.MarkupLine(Colors.Red($"Resource {settings.ResourceID} was not found"));
                return -1;
            }

            if (!languages.Contains(settings.LangCode))
            {
                AnsiConsole.MarkupLine(Colors.Red($"Language {settings.LangCode} was not found"));
                return -1;
            }

            resourcesDict[settings.LangCode].Data[settings.ResourceID] = settings.Text;
            Resources.UpdateResource(resourcesDict);

            AnsiConsole.MarkupLine(Colors.Green(settings.LangCode) + $" - {settings.Text}");

            return 0;
        }
    }
}
