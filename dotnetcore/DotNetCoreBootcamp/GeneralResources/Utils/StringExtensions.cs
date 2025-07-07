using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GeneralResources.Utils
{
    public static class StringExtensions
    {
        public static string Dump(this string value)
        {
            Debug.Print(value);
            return value;
        }
        
    }
}