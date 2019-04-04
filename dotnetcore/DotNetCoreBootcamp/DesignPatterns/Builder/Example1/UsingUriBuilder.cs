using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Builder.Example1
{
    public class UsingUriBuilder
    {
        public static string DotNetExample(string scheme, string host, int portNumber)
        {
            UriBuilder builder = new UriBuilder();
            builder.Scheme = scheme;
            builder.Host = host;
            builder.Port = portNumber;

            return builder.ToString();
        }
    }
}
