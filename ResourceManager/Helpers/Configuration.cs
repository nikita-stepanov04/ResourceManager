using ResourceManager.Models;
using Spectre.Console;
using System.Text.Json;

namespace ResourceManager.Helpers
{
    public static class Configuration
    {
        public static string CONFIG_FOLDER_NAME = "ResourceManagerConfig";
        public static string CONFIG_FILE_NAME = "config.json";

        public static Config? GetConfig()
        {
            var documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var configFolder = Path.Combine(documentsFolder, CONFIG_FOLDER_NAME);

            var configFile = Path.Combine(configFolder, CONFIG_FILE_NAME);

            if (!Path.Exists(configFile))
            {
                AnsiConsole.MarkupLine($"[red]File {configFile} was not found[/]");
                return null;
            }

            return JsonSerializer.Deserialize<Config>(File.ReadAllText(configFile))!;
        }
        
    }
}
