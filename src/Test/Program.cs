using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlPageBuilder;
using WatsonWebserver;
using WatsonWebserver.Core;
using static WatsonWebserver.Core.WebserverPages;

namespace Test
{
    class Program
    {
        static string _Hostname = "localhost";
        static int _Port = 9000;
        static bool _Ssl = false;

        static Webserver _Webserver = null;
        static HtmlPage _Page = null;
        static string _PageString = null;

        static void Main()
        {
            _Page = new HtmlPage();
            
            _Page.Head.Title = "My Page";

            _Page.Head.FaviconUrl = "./assets/icon.ico";

            _Page.Head.MetaKeywords = "my,page";
            
            _Page.Head.Style = "h1 { font-family: 'arial' } p { font-family: 'arial' } ul { font-family: 'arial' }";

            _Page.Body.Content += _Page.Body.H1Text("My Page");

            _Page.Body.Content += _Page.Body.HorizontalRule();

            _Page.Body.Content += _Page.Body.Paragraph("This is some sample text.");

            _Page.Body.Content += _Page.Body.Paragraph("This is a button within a paragraph that links to Github!");

            _Page.Body.Content += _Page.Body.Paragraph(
                _Page.Body.Link(
                    _Page.Body.Button("Github"),
                    "https://github.com/jchristn/htmlpagebuilder",
                    true)
                );
            
            _Page.Body.Content += _Page.Body.Paragraph("Here are a series of buttons next to one another!<br />");

            _Page.Body.Content += _Page.Body.Paragraph(
                _Page.Body.Button(
                    _Page.Body.Link("Google", "https://google.com", true, null, null, "text-decoration: none; color: white;")
                    ) + " " +
                _Page.Body.Button(
                    _Page.Body.Link("Youtube", "https://youtube.com", true, null, null, "text-decoration: none; color: white;")
                    ) + " " +
                _Page.Body.Button(
                    _Page.Body.Link("CNN", "https://cnn", true, null, null, "text-decoration: none; color: white;")
                    )
                );

            _Page.Body.Content += _Page.Body.UnorderedList(new List<string> { "foo", "bar", "baz" });
            
            _PageString = _Page.ToString();

            _Webserver = new Webserver(new WebserverSettings(_Hostname, _Port, _Ssl), DefaultRoute);
            _Webserver.Routes.PreAuthentication.Content.Add("/assets/", true);
            _Webserver.Start();

            Console.WriteLine("Listening on http://localhost:9000, press ENTER to exit");
            Console.ReadLine();
        }

        static async Task DefaultRoute(HttpContextBase ctx)
        {
            ctx.Response.ContentType = "text/html";
            ctx.Response.ContentLength = _PageString.Length;
            await ctx.Response.Send(_PageString);
        }
    }
}
