using Google.Api.Gax.ResourceNames;
using Google.Cloud.Translate.V3;
using System.Net;

namespace ResourceManager.Helpers
{
    public static class Translator
    {
        const string TRANSLATOR_KEY_FILE_NAME = "key.json";
        const string CONFIG_FOLDER_NAME = "ResourceManagerConfig";

        public static async Task<Dictionary<string, string>?> TranslateAsync(List<string> languages, string text)
        {
            var config = Configuration.GetConfig();
            if (config == null)
                return null;

            languages.Remove(config.MainLanguage);

            var documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var configFolder = Path.Combine(documentsFolder, CONFIG_FOLDER_NAME);
            var translatorKeyFile = Path.Combine(configFolder, TRANSLATOR_KEY_FILE_NAME);

            var client = await new TranslationServiceClientBuilder
            {
                CredentialsPath = translatorKeyFile
            }.BuildAsync();

            var tasksDict = languages.ToDictionary(
                language => language,
                language =>
                {
                    var request = new TranslateTextRequest
                    {
                        Contents = { text },
                        TargetLanguageCode = language,
                        SourceLanguageCode = config.MainLanguage,
                        Parent = new ProjectName(config.TranslatorProjectID).ToString()
                    };
                    return client.TranslateTextAsync(request);
                });

            await Task.WhenAll(tasksDict.Values);

            var res = tasksDict.ToDictionary(
                kvp => kvp.Key,
                kvp => WebUtility.HtmlDecode(kvp.Value.Result.Translations.First().TranslatedText));
            res.Add(config.MainLanguage, text);
            return res;
        }
    }
}
