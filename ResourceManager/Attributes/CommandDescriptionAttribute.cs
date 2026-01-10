namespace ResourceManager.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CommandDescriptionAttribute : Attribute
    {
        public string CommandDescription { get; } = null!;

        public CommandDescriptionAttribute(string commandDescription)
        {
            CommandDescription = commandDescription;
        }
    }
}
