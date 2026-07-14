using ResourceManager.Attributes;
using ResourceManager.Helpers;
using ResourceManager.Settings;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ResourceManager.Commands
{
    [CommandName("add", ordering: 2)]
    [CommandDescription("Add a new resource in the main language from config and translate it into other languages")]
    public class AddResourceCommand : AsyncCommand<ResourceSettings>
    {
        protected override async Task<int> ExecuteAsync(
            CommandContext context, ResourceSettings settings, CancellationToken token)
        {
            var config = Configuration.GetConfig();
            if (config == null)
                return -1;

            var (resourcesDict, languages) = Resources.GetResources(config.ResourcesFolder);
            if (resourcesDict == null)
                return -1;

            var duplicate = resourcesDict[config.MainLanguage].Data.FirstOrDefault(kvp => kvp.Value == settings.Text);
            if (!string.IsNullOrEmpty(duplicate.Key))
            {
                AnsiConsole.MarkupLine("Resource already exists: " + Colors.Blue(Resources.AngularTemplate(duplicate.Key)));
                return -1;
            }

            var translations = await Translator.TranslateAsync(languages, [settings.Text]);
            if (translations == null)
                return -1;

            var resourceID = Resources.GetNewResourceName(resourcesDict[config.MainLanguage].Data);

            foreach (var kvp in resourcesDict)
            {
                var lang = kvp.Key;
                var resource = kvp.Value.Data;

                var translatedText = translations[lang].First();
                resource.Add(resourceID, translatedText);
                AnsiConsole.MarkupLine(Colors.Green(lang) + $" - {translatedText}");
            }

            Resources.UpdateResource(resourcesDict);

            AnsiConsole.MarkupLine("New resource: " + Colors.Blue(resourceID));
            return 0;
        }
    }
}
