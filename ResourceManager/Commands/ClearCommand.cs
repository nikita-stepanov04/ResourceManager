using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceManager.Commands
{
    internal class ClearCommand : Command
    {
        public override int Execute(CommandContext context, CancellationToken cancellationToken)
        {
            Console.Clear();
            return 0;
        }
    }
}
