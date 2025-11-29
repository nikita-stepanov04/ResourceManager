using ResourceManager.Commands;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Text.RegularExpressions;

namespace ResourceManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new CommandApp();

            app.Configure(config =>
            {
                config.AddCommand<AddResourceCommand>("add")
                    .WithDescription("Add a new resource in the main language from config and translate it into other languages");
                config.AddCommand<GetResourceCommand>("get")
                    .WithDescription("Display resources by resource ID");
                config.AddCommand<UpdateResourceCommand>("update")
                    .WithDescription("Update a resource by resource ID with a new value and translate it into other languages");
                config.AddCommand<EditResourceCommand>("edit")
                    .WithDescription("Edit a resource by resource ID and language code with a new value, other languages remain unchanged");
                config.AddCommand<ClearCommand>("clear")
                    .WithAlias("cls")
                    .WithDescription("Clear console");
            });

            Console.Clear();
            while (true)
            {                
                AnsiConsole.Markup("[fuchsia]> [/]");
                var line = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (line.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    break;

                if (line.Equals("help", StringComparison.OrdinalIgnoreCase))
                    line = "--help";

                var matches = Regex.Matches(line, @"[\""].+?[\""]|[^ ]+");

                var inputArgs = matches
                    .Select(m => m.Value.Trim('"'))
                    .ToArray();

                app.Run(inputArgs);
            }
        }
    }
}
