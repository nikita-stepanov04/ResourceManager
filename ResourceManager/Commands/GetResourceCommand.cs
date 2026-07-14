using ResourceManager.Attributes;
using ResourceManager.Helpers;
using ResourceManager.Settings;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ResourceManager.Commands
{
    [CommandName("get", ordering: 1)]
    [CommandDescription("Display resources by resource ID")]
    public class GetResourceCommand : Command<GetResourceSetting>
    {
        protected override int Execute(
            CommandContext context, GetResourceSetting settings, CancellationToken cancellationToken)
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

            foreach (var language in languages) 
            {
                var resources = resourcesDict[language].Data;
                var resource = resources[settings.ResourceID];

                AnsiConsole.MarkupLine(Colors.Green(language) + $" - {resource}");
            }

            return 0;
        }
    }
}
