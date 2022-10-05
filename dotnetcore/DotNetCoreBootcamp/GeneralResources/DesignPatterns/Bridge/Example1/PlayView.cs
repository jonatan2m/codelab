using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Bridge.Example1
{
    public class PlayView
    {
        public static void Run()
        {
            var artist = new Artist()
            {
                Bio = "Bio",
                Image = "image",
                Name = "Artist's name",
                Url = "http://aaa"
            };

            View view = new LongForm(new ArtistResource(artist));
            Client client = new Client();
            client.ClientCode(view);
        }
    }
}
