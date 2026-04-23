using ResourceManager.Helpers;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Text;
using System.Text.RegularExpressions;

namespace ResourceManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            var app = new CommandApp();

            app.Configure(ConfigureApp.Configure);

            while (true)
            {
                AnsiConsole.Markup(Colors.Fuchsia("> "));
                var line = Console.ReadLine()!.Trim("--").ToString();

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (line.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    break;

                var matches = Regex.Matches(line, @"[\""].+?[\""]|[^ ]+");

                var inputArgs = matches
                    .Select(m => m.Value.Trim('"'))
                    .ToArray();

                app.Run(inputArgs);
            }
        }
    }
}
