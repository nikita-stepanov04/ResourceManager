namespace ResourceManager.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CommandNameAttribute : Attribute
    {
        public string CommandName { get; } = null!;
        public int Ordering { get; set; }

        public CommandNameAttribute(string commandName, int ordering)
        {
            this.CommandName = commandName;
            this.Ordering = ordering;
        }
    }
}
