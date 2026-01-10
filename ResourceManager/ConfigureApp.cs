using ResourceManager.Attributes;
using Spectre.Console.Cli;
using System.Data;
using System.Reflection;

namespace ResourceManager
{
    public static class ConfigureApp
    {
        public static void Configure(IConfigurator config)
        {
            var types = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Namespace != null && t.Namespace.StartsWith(typeof(Program).Namespace!))
                .Select(t => (
                    Type: t,
                    CommandNameAttr: t.GetCustomAttribute<CommandNameAttribute>(),
                    CommandAliasAttrs: t.GetCustomAttributes<CommandAliasAttribute>(),
                    CommandDescriptionAttr: t.GetCustomAttribute<CommandDescriptionAttribute>()
                ))
                .Where(x => x.CommandNameAttr != null);

            var addCommandMethod = config.GetType().GetMethod("AddCommand");
            foreach (var typeInfo in types)
            {
                var commandConfig = (ICommandConfigurator) addCommandMethod!
                    .MakeGenericMethod(typeInfo.Type)
                    .Invoke(config, [typeInfo.CommandNameAttr!.CommandName])!;

                if (typeInfo.CommandDescriptionAttr != null)
                    commandConfig.WithDescription(typeInfo.CommandDescriptionAttr.CommandDescription);

                if (typeInfo.CommandAliasAttrs != null)
                {
                    foreach (var aliasAttr in typeInfo.CommandAliasAttrs)
                    {
                        commandConfig.WithAlias(aliasAttr.CommandAlias);
                    }
                }
            }
        }
    }
}
