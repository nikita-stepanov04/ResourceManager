using ResourceManager.Attributes;
using Spectre.Console.Cli;

namespace ResourceManager.Commands
{
    [CommandAlias("cls")]
    [CommandName("clear")]
    [CommandDescription("Clear console")]
    public class ClearCommand : Command
    {
        public override int Execute(CommandContext context, CancellationToken cancellationToken)
        {
            Console.Clear();
            return 0;
        }
    }
}
