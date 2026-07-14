using ResourceManager.Attributes;
using ResourceManager.Helpers;
using ResourceManager.Settings;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Resources.NetStandard;


namespace ResourceManager.Commands
{
    [CommandName("resx-port", ordering: 8)]
    [CommandDescription(
        "Convert all resource files to .resx format in the target folder. " +
        "The base file AppResources.resx represents the NeutralCulture (default language). " +
        "Localized files are generated as AppResources.{culture}.resx"        
    )]
    public class PortToResxCommand : Command<PortToResxSetting>
    {
        const string RESX_FILE_NAME_FORMAT = "AppResources{0}.resx";

        protected override int Execute(
            CommandContext context, PortToResxSetting settings, CancellationToken cancellationToken)
        {
            var config = Configuration.GetConfig();
            if (config == null)
                return -1;

            var (resourcesDict, languages) = Resources.GetResources(config.ResourcesFolder);
            if (resourcesDict == null)
                return -1;

            if (!languages.Contains(settings.NeutralCulture))
            {
                AnsiConsole.MarkupLine(Colors.Red($"{settings.NeutralCulture} " +
                    $"lang is not found is the languages list"));
                return -1;
            }

            Directory.CreateDirectory(settings.AbsoluteDestPath);

            Array.ForEach(Directory.GetFiles(settings.AbsoluteDestPath, "*.resx"),
                file => File.Delete(file));

            foreach (var resources in resourcesDict)
            {
                var resxFilePath = Path.Combine(settings.AbsoluteDestPath, 
                    settings.NeutralCulture == resources.Key
                        ? string.Format(RESX_FILE_NAME_FORMAT, string.Empty)
                        : string.Format(RESX_FILE_NAME_FORMAT, $".{resources.Key}"));

                using (var rw = new ResXResourceWriter(resxFilePath))
                {
                    foreach (var resource in resources.Value.Data)
                    {
                        rw.AddResource(resource.Key, resource.Value);
                    }
                }
            }

            return 0;
        }
    }
}
