using ResourceManager.Attributes;
using ResourceManager.Helpers;
using ResourceManager.Settings;
using Spectre.Console.Cli;
using System.Resources.NetStandard;


namespace ResourceManager.Commands
{
    [CommandName("resx-port", ordering: 8)]
    [CommandDescription("Port all resources files to resx format in the specified folder")]
    public class PortToResxCommand : Command<PortToResxSetting>
    {
        const string RESX_FILE_NAME_FORMAT = "AppResources.{0}.resx";

        protected override int Execute(
            CommandContext context, PortToResxSetting settings, CancellationToken cancellationToken)
        {
            var config = Configuration.GetConfig();
            if (config == null)
                return -1;

            var (resourcesDict, languages) = Resources.GetResources(config.ResourcesFolder);
            if (resourcesDict == null)
                return -1;

            Directory.CreateDirectory(settings.AbsoluteDestPath);

            Array.ForEach(Directory.GetFiles(settings.AbsoluteDestPath, "*.resx"),
                file => File.Delete(file));

            foreach (var resources in resourcesDict)
            {
                var resxFilePath = Path.Combine(settings.AbsoluteDestPath,
                    string.Format(RESX_FILE_NAME_FORMAT, resources.Key));

                var a = resources.Value.Data;

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
