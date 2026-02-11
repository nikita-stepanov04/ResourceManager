using ResourceManager.Attributes;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ResourceManager.Commands
{
    [CommandName("help", ordering: -1)]
    public class HelpCommand : Command
    {
        public override int Execute(CommandContext context, CancellationToken cancellationToken)
        {
            var commandsInfo = ConfigureApp.CommandsInfo;

            var table = new Table()
                .RoundedBorder()
                .ShowRowSeparators()
                .AddColumn("[yellow]Command[/]")
                .AddColumn("[yellow]Parameters[/]")
                .AddColumn("[yellow]Description[/]");

            foreach (var commandInfo in commandsInfo)
            {
                var paramsGrid = new Grid()
                    .AddColumn(new GridColumn().LeftAligned().Width(12))
                    .AddColumn(new GridColumn().RightAligned());

                foreach (var param in commandInfo.Parameters)
                {
                    paramsGrid.AddRow(
                        param.Name,
                        param.IsRequired ? "[red]req[/]" : "[green]nonreq[/]"
                    );
                }

                table.AddRow(
                    new Markup(commandInfo.Name),
                    paramsGrid,
                    new Markup(commandInfo.Description ?? "")
                );
            }

            AnsiConsole.Write(table);
            return 0;
        }
    }
}
