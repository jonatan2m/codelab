namespace DesignPatterns.Command.Example4
{
    internal class RemoveTextCommand : BaseCommand
    {
        public RemoveTextCommand(ApplicationMakeText app, TextEditor editor) : base(app, editor)
        {
        }

        public override bool Execute()
        {
            SaveBackup();
            editor.Remove(app.CurrentText);
            return true;
        }
    }
}