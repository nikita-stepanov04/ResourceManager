using ResourceManager.Helpers;
using ResourceManager.Settings;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ResourceManager.Commands
{
    public class GetResourceCommand : Command<GetResourceSetting>
    {
        public override int Execute(CommandContext context, GetResourceSetting settings, CancellationToken cancellationToken)
        {
            var config = Configuration.GetConfig();
            if (config == null)
                return -1;

            var (resourcesDict, languages) = Resources.GetResources(config.ResourcesFolder);
            if (resourcesDict == null)
                return -1;

            foreach(var language in languages)
            {
                var resources = resourcesDict[language].Data;
                var resource = resources[settings.ResourceID];

                AnsiConsole.MarkupLine($"[green]{language}[/][white] - {resource}[/]");
            }

            return 0;
        }
    }
}
