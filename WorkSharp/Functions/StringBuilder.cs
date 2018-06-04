using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace WorkSharp.Functions
{
    public static class StringBuilder
    {

        public static string JoinArray(dynamic Result)
        {

            return (Result as HtmlNodeCollection)
                .Select(x => x.InnerText
                    .Replace("\n","")
                    .Replace("\r", "")
                    )
                .Aggregate((a, b) => a + " " + b);

        } 


    }
}
