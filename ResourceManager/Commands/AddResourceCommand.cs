using ResourceManager.Helpers;
using ResourceManager.Settings;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ResourceManager.Commands
{
    public class AddResourceCommand : AsyncCommand<ResourceSettings>
    {
        public override async Task<int> ExecuteAsync(CommandContext context, ResourceSettings settings, CancellationToken token)
        {
            var config = Configuration.GetConfig();
            if (config == null)
                return -1;

            var (resourcesDict, languages) = Resources.GetResources(config.ResourcesFolder);
            if (resourcesDict == null)
                return -1;

            var translations = await Translator.TranslateAsync(languages, settings.Text);
            if (translations == null)
                return -1;

            var resourceID = Resources.GetNewResourceName(resourcesDict[config.MainLanguage].Data);

            foreach (var kvp in resourcesDict)
            {
                var lang = kvp.Key;
                var resource = kvp.Value.Data;

                var translatedText = translations[lang];
                resource.Add(resourceID, translatedText);
                AnsiConsole.MarkupLine($"[green]{lang}[/] - {translatedText}");
            }

            Resources.UpdateResource(resourcesDict);

            AnsiConsole.MarkupLine($"New resource id: [blue]{resourceID}[/]");
            return 0;
        }
    }
}
