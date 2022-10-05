namespace DesignPatterns.Bridge.Example1
{
    public interface IResource
    {
        string Snippet();
        string Title();
        string Image();
        string Url();
    }

    public class ArtistResource : IResource
    {
        Artist artist;
        public ArtistResource(Artist artist)
        {
            this.artist = artist;
        }

        public string Image()
        {
            return artist.Image;
        }

        public string Snippet()
        {
            return artist.Bio;
        }

        public string Title()
        {
            return artist.Name;
        }

        public string Url()
        {
            return artist.Url;
        }
    }
}