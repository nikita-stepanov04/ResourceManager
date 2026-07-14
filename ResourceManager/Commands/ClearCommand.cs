using ResourceManager.Attributes;
using Spectre.Console.Cli;

namespace ResourceManager.Commands
{
    [CommandAlias("cls")]
    [CommandName("clear", ordering: 7)]
    [CommandDescription("Clear console")]
    public class ClearCommand : Command
    {
        protected override int Execute(CommandContext context, CancellationToken cancellationToken)
        {
            Console.Clear();
            return 0;
        }
    }
}
