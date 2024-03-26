using Data;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using static JavBusDownloader.WebView;

namespace JavBusDownloader
{
    public partial class Genre : Form
    {
        bool isNormal = true;
        public WebViewMain webviewMain;

        public Genre(string str)
        {
            InitializeComponent();
            isNormal = str == "normal";
        }

        private void webView21_Click(object sender, EventArgs e)
        {
           
        }

        private async void Genre_Load(object sender, EventArgs e)
        {
            webviewMain = new WebViewMain(webView21, new WebViewObj(this, webView21));
            webviewMain.LoadWebview();

            webviewMain.webview.NavigationCompleted += ((WebViewObj)webviewMain.obj).StartAsync;
            string str = $"http://sljly.xyz/{(isNormal ? "genre" : "un-genre")}.html";
            if (await webviewMain.Start($"\\Web", str))
            {
                webView21.CoreWebView2.AddWebResourceRequestedFilter("*", CoreWebView2WebResourceContext.All);
                webView21.CoreWebView2.WebResourceRequested += delegate (object sender, CoreWebView2WebResourceRequestedEventArgs args)
                {
                    args.Request.Headers.SetHeader("Referer", "www.javbus.com");
               
                };
                webView21.CoreWebView2.NavigationStarting += (sender, args) =>
                {
                    string mainString = HttpUtility.UrlDecode(args.Uri);
                    List<string> subStrings = new List<string> { "sljly.xyz/genre/", "sljly.xyz/uncensored/genre/" };

                    foreach (string subString in subStrings)
                    {
                        int index = mainString.IndexOf(subString, StringComparison.OrdinalIgnoreCase);
                        if (index >= 0)
                        {
                            string[] afterMatch = mainString.Substring(index + subString.Length).Split('|');
                            Core.NewForm(subString == "sljly.xyz/genre/" ? MovieMod.normal : MovieMod.uncensored, SearchMod.genre, afterMatch[0], afterMatch[1]);
                            this.Close();
                        }
                    };
                    if ( mainString.IndexOf("genre.html", StringComparison.OrdinalIgnoreCase) < 0 )
                    {
                        args.Cancel = true;
                    }
                };
            };
        }
    }
}
