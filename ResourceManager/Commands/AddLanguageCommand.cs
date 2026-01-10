using ResourceManager.Attributes;
using ResourceManager.Helpers;
using ResourceManager.Settings;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace ResourceManager.Commands
{
    [CommandName("add-language")]
    [CommandDescription("Add resource language and automatically translates all existing resources from main language to new one")]
    public class AddLanguageCommand : AsyncCommand<NewLanguageSetting>
    {
        public override async Task<int> ExecuteAsync(CommandContext context, NewLanguageSetting settings, CancellationToken cancellationToken)
        {
            var config = Configuration.GetConfig();
            if (config == null)
                return -1;

            var (resourcesDict, languages) = Resources.GetResources(config.ResourcesFolder);
            if (resourcesDict == null)
                return -1;

            if (languages.Contains(settings.LandCode))
            {
                AnsiConsole.MarkupLine($"[red]Language {settings.LandCode} is already added[/]");
                return -1;
            }

            if (!await Translator.CheckLangCodeValidity(settings.LandCode))
            {
                AnsiConsole.MarkupLine($"[red]Language {settings.LandCode} is not a ISO language[/]");
                return -1;
            }

            var defaultLangResources = resourcesDict[config.MainLanguage].Data.Select(kvp => kvp.Value).ToList();

            var translations = await Translator.TranslateAsync([settings.LandCode], defaultLangResources);

            if (translations == null)
                return -1;

            var translatedTexts = translations[settings.LandCode];

            var json = translatedTexts.Index().ToDictionary(
                data => Resources.GetNewResourceName(data.Index + 1),
                data => data.Item);

            var filePath = Path.Combine(config.ResourcesFolder, settings.LandCode + ".json");
            File.WriteAllText(filePath, JsonSerializer.Serialize(json, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }));

            foreach (var kvp in json)
            {
                AnsiConsole.MarkupLine($"[green]{kvp.Key}[/][white] - {kvp.Value}[/]");
            }

            return 0;
        }
    }
}
