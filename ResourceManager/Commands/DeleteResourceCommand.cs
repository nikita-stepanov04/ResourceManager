using ResourceManager.Attributes;
using ResourceManager.Helpers;
using ResourceManager.Settings;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ResourceManager.Commands
{
    [CommandName("delete")]
    [CommandDescription("Delete a resource by resource ID")]
    public class DeleteResourceCommand : Command<GetResourceSetting>
    {
        public override int Execute(CommandContext context, GetResourceSetting settings, CancellationToken cancellationToken)
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

            foreach (var kvp in resourcesDict)
            {
                var lang = kvp.Key;
                var resource = kvp.Value.Data;
                resource.Remove(settings.ResourceID);
            }
            Resources.UpdateResource(resourcesDict);

            AnsiConsole.MarkupLine($"Resource [red]{settings.ResourceID}[/] was deleted");

            return 0;
        }
    }
}
