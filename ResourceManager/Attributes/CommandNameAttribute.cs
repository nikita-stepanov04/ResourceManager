namespace ResourceManager.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CommandNameAttribute : Attribute
    {
        public string CommandName { get; } = null!;

        public CommandNameAttribute(string commandName)
        {
            this.CommandName = commandName;
        }
    }
}
