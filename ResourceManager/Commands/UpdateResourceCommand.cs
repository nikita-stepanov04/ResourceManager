using ResourceManager.Helpers;
using ResourceManager.Settings;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ResourceManager.Commands
{
    public class UpdateResourceCommand : AsyncCommand<UpdateResourceSetting>
    {
        public override async Task<int> ExecuteAsync(CommandContext context, UpdateResourceSetting settings, CancellationToken cancellationToken)
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

            var translations = await Translator.TranslateAsync(languages, settings.Text);
            if (translations == null)
                return -1;

            foreach (var kvp in resourcesDict)
            {
                var lang = kvp.Key;
                var resource = kvp.Value.Data;

                var translatedText = translations[lang];
                resource[settings.ResourceID] = translatedText;
                AnsiConsole.MarkupLine($"[green]{lang}[/] - {translatedText}");
            }
            Resources.UpdateResource(resourcesDict);

            return 0;
        }
    }
}
