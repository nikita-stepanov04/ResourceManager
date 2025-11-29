using ResourceManager.Helpers;
using ResourceManager.Settings;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ResourceManager.Commands
{
    public class EditResourceCommand : Command<EditResourceSetting>
    {
        public override int Execute(CommandContext context, EditResourceSetting settings, CancellationToken cancellationToken)
        {
            var config = Configuration.GetConfig();
            if (config == null)
                return -1;

            var (resourcesDict, languages) = Resources.GetResources(config.ResourcesFolder);
            if (resourcesDict == null)
                return -1;

            if (!resourcesDict.Values.First().Data.ContainsKey(settings.ResourceID))
            {
                AnsiConsole.MarkupLine($"[red]Resource {settings.ResourceID} was not found[/]");
                return -1;
            }

            if (!languages.Contains(settings.LangCode))
            {
                AnsiConsole.MarkupLine($"[red]Language {settings.LangCode} was not found[/]");
                return -1;
            }

            resourcesDict[settings.LangCode].Data[settings.ResourceID] = settings.Text;
            Resources.UpdateResource(resourcesDict);

            AnsiConsole.MarkupLine($"[green]{settings.LangCode}[/][white] - {settings.Text}[/]");

            return 0;
        }
    }
}
