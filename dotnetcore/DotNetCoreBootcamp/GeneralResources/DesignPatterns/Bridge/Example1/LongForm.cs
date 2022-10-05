namespace DesignPatterns.Bridge.Example1
{
    public class LongForm : View
    {
        public LongForm(IResource resource) : base(resource)
        { }

        public override string Show()
        {
            // make some modifications
            var snippet = resource.Snippet();
            // create HTML
            var html = "" + snippet;
            return html;            
        }
    }
}
