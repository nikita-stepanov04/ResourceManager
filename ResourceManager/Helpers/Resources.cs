using Spectre.Console;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace ResourceManager.Helpers
{
    public static class Resources
    {
        const string RESOURCE_ID_PREFIX = "str";

        public static (Dictionary<string, ResourceInfo>?, List<string>) GetResources(string path)
        {
            try
            {
                var resourcesDict = Directory
                    .GetFiles(path)
                    .ToDictionary(
                        filePath => Path.GetFileNameWithoutExtension(filePath),
                        filePath =>
                        {
                            var text = File.ReadAllText(filePath);

                            return new ResourceInfo
                            {
                                Path = filePath,
                                Data = string.IsNullOrWhiteSpace(text)
                                    ? new Dictionary<string, string>()
                                    : JsonSerializer.Deserialize<Dictionary<string, string>>(text)
                                        ?? new Dictionary<string, string>()
                            };
                        }
                    );
                var languages = resourcesDict.Keys.ToList();
                return (resourcesDict, languages);
            }
            catch
            {
                AnsiConsole.MarkupLine($"[red]Failed to load resources[/]");
                return (null, []);
            }
        }

        public static string GetNewResourceName(Dictionary<string, string> data)
        {
            return RESOURCE_ID_PREFIX + (int.Parse(
                data.Keys
                .Order()
                .LastOrDefault()?
                .Replace(RESOURCE_ID_PREFIX, string.Empty) ?? "0"
            ) + 1).ToString("D3");
        }

        public static void UpdateResource(Dictionary<string, ResourceInfo> resourcesDict)
        {
            foreach (var kvp in resourcesDict)
            {
                var resource = kvp.Value.Data;

                File.WriteAllText(
                    path: kvp.Value.Path,
                    encoding: Encoding.UTF8,
                    contents: JsonSerializer.Serialize(resource, new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping                    
                    })
                );
            }
        }
    }

    public class ResourceInfo
    {
        public string Path { get; set; } = null!;
        public Dictionary<string, string> Data { get; set; } = null!;
    }
}
