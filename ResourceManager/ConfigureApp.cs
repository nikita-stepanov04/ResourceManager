using ResourceManager.Attributes;
using Spectre.Console.Cli;
using System.Data;
using System.Reflection;

namespace ResourceManager
{
    public static class ConfigureApp
    {
        public static IEnumerable<CommandInfo> CommandsInfo { get; private set; } = [];

        public static void Configure(IConfigurator config)
        {
            var typesInfo = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Namespace != null && t.Namespace.StartsWith(typeof(Program).Namespace!))
                .Select(t => (
                    Type: t,
                    CommandNameAttr: t.GetCustomAttribute<CommandNameAttribute>(),
                    CommandAliasAttrs: t.GetCustomAttributes<CommandAliasAttribute>(),
                    CommandDescriptionAttr: t.GetCustomAttribute<CommandDescriptionAttribute>()
                ))
                .Where(x => x.CommandNameAttr != null)
                .ToList();

            var a = typesInfo
                .Where(ti => ti.CommandNameAttr!.Ordering > 0)
                .OrderBy(ti => ti.CommandNameAttr!.Ordering)
                .ToList();

            CommandsInfo = typesInfo
                .Where(ti => ti.CommandNameAttr!.Ordering > 0)
                .OrderBy(ti => ti.CommandNameAttr!.Ordering)
                .Select(t => new CommandInfo
                {
                    Name = t.CommandNameAttr!.CommandName,
                    Description = t.CommandDescriptionAttr?.CommandDescription,
                    Aliases = t.CommandAliasAttrs.Select(a => a.CommandAlias),
                    Parameters = (
                        t.Type.GetMethod("Execute", BindingFlags.Instance | BindingFlags.NonPublic) ??
                        t.Type.GetMethod("ExecuteAsync", BindingFlags.Instance | BindingFlags.NonPublic)
                    )!
                    .GetParameters()
                    .Where(p => typeof(CommandSettings).IsAssignableFrom(p.ParameterType))
                    .Select(pi => pi.ParameterType
                        .GetProperties()
                        .Select(prp => prp.GetCustomAttribute<CommandArgumentAttribute>())
                        .OrderBy(atr => atr!.Position)
                        .Select(atr => new CommandParameterInfo
                        {
                            Name = atr!.ValueName,
                            IsRequired = atr!.IsRequired
                        })
                    ).FirstOrDefault() ?? []
                }).ToList();

            var addCommandMethod = config.GetType().GetMethod("AddCommand");
            foreach (var typeInfo in typesInfo)
            {
                var commandConfig = (ICommandConfigurator)addCommandMethod!
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

    public class CommandInfo
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public IEnumerable<string> Aliases { get; set; } = [];
        public IEnumerable<CommandParameterInfo> Parameters { get; set; } = [];
    }

    public class CommandParameterInfo
    {
        public string Name { get; set; } = null!;
        public bool IsRequired { get; set; }
    }
}
