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
