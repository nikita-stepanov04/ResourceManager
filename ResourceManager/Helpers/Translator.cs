using Google.Api.Gax.ResourceNames;
using Google.Cloud.Translate.V3;
using ResourceManager.Models;
using System.Net;

namespace ResourceManager.Helpers
{
    public static class Translator
    {
        const string TRANSLATOR_KEY_FILE_NAME = "key.json";

        public static async Task<Dictionary<string, List<string>>?> TranslateAsync(
            List<string> languages,
            List<string> texts)
        {
            var config = Configuration.GetConfig();
            if (config == null)
                return null;

            var client = BuildClient(config);

            languages.Remove(config.MainLanguage);            

            var tasksDict = languages.ToDictionary(
                language => language,
                language =>
                {
                    var request = new TranslateTextRequest
                    {
                        Contents = { texts },
                        TargetLanguageCode = language,
                        SourceLanguageCode = config.MainLanguage,
                        Parent = new ProjectName(config.TranslatorProjectID).ToString()
                    };
                    return client.TranslateTextAsync(request);
                });

            await Task.WhenAll(tasksDict.Values);

            var res = tasksDict.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Result.Translations
                    .Select(t =>
                    {
                        var decoded = WebUtility.HtmlDecode(t.TranslatedText);
                        if (decoded.EndsWith('.') && !decoded.EndsWith("..."))
                            return decoded.TrimEnd('.');
                        return decoded;
                    })
                    .ToList());

            res.Add(config.MainLanguage, texts);
            return res;
        }

        public static async Task<bool> CheckLangCodeValidity(string langCode)
        {
            var config = Configuration.GetConfig();
            if (config == null)
                return false;

            var client = BuildClient(config);

            try
            {
                var request = new GetSupportedLanguagesRequest
                {
                    Parent = new ProjectName(config.TranslatorProjectID).ToString(),
                    DisplayLanguageCode = langCode
                };

                var response = client.GetSupportedLanguages(request);
                return response.Languages.Any(lang => lang.LanguageCode == langCode);
            }
            catch { return false; }
        }

        private static TranslationServiceClient BuildClient(Config config)
        {
            var documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var configFolder = Path.Combine(documentsFolder, Configuration.CONFIG_FOLDER_NAME);
            var translatorKeyFile = Path.Combine(configFolder, TRANSLATOR_KEY_FILE_NAME);

            return new TranslationServiceClientBuilder
            {
                CredentialsPath = translatorKeyFile
            }.Build();
        }
    }
}
