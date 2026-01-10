namespace ResourceManager.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CommandAliasAttribute : Attribute
    {
        public string CommandAlias { get; } = null!;

        public CommandAliasAttribute(string commandAlias)
        {
            CommandAlias = commandAlias;
        }
    }
}
