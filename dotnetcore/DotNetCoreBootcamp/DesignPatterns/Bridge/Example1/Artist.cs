namespace DesignPatterns.Bridge.Example1
{
    public class Artist
    {
        public string Bio { get; set; }
        public string Name { get; set; }
        public string Image { get; internal set; }
        public string Url { get; internal set; }
    }
}