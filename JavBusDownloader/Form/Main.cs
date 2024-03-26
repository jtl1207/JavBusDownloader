using Data;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static JavBusDownloader.WebView;

namespace JavBusDownloader
{
    public partial class Main : Form
    {
        public WebViewMain webviewMain;
        public SearchMod searchMod = SearchMod.search;
        public MovieMod type = MovieMod.normal;
        public string search = string.Empty;
        public int page = 0;
        public bool HasNextPage = false;
        public string MoviesJson;

        public Main()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, System.EventArgs e)
        {
            webviewMain.webview.CoreWebView2.ExecuteScriptAsync($"获取实例();");
        }

        private async void Main_Load(object sender, EventArgs e)
        {
            webviewMain = new WebViewMain(webView21, new WebViewObj(this, webView21));
            webviewMain.LoadWebview();

            webviewMain.webview.NavigationCompleted += ((WebViewObj)webviewMain.obj).StartAsync;
            string str = "http://sljly.xyz/index.html";
            if (await webviewMain.Start($"\\Web", str))
            {
                webView21.CoreWebView2.AddWebResourceRequestedFilter("*", CoreWebView2WebResourceContext.All);
                webView21.CoreWebView2.WebResourceRequested += delegate (
                       object sender, CoreWebView2WebResourceRequestedEventArgs args)
                {
                    args.Request.Headers.SetHeader("Referer", "www.javbus.com");
                };
            }
            new Thread(SearchThread) { IsBackground = true }.Start();
        }


        #region WebView
        private async Task WebViewLoad(Main main)
        {
            webviewMain.webview.NavigationCompleted += ((WebViewObj)webviewMain.obj).StartAsync;
            string str = "http://sljly.xyz/index.html";
            if (await webviewMain.Start($"\\Web", str))
            {
                main.webView21.CoreWebView2.AddWebResourceRequestedFilter("*", CoreWebView2WebResourceContext.All);
            }
        }
        #endregion


        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Core.CloseForm(Name);
        }

        private void SearchThread()
        {
            bool inSearchNextPage = false;
            while (true)
            {
                if (this == null) return;
                if (!Core.movieList.ContainsKey(Name)) continue;
                try
                {
                    foreach (Movies movie in Core.movieList[Name].Values)
                    {
                        if (!movie.inSearch)
                        {
                            movie.inSearch = true;
                            if (movie.state == 0)
                            {
                                Core.threadPool.QueueUserWorkItem(new Action(() =>
                                {
                                    try
                                    {
                                        if (!Core.movieList.ContainsKey(Name)) return;
                                        string str1 = Name;
                                        string str2 = movie.id;
                                        Core.movieInfoList[str1][str2].movieInfo = OpenAPI.GetMovieInfo(Settings.API, str2);
                                        if (Core.movieInfoList[str1][str2].movieInfo != null)
                                        {
                                            Core.movieList[str1][str2].state = 1;
                                        }
                                        Core.movieList[str1][str2].inSearch = false;
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.Message);
                                    }
                                }), 10);
                            }
                            if (movie.state == 1)
                            {
                                Core.threadPool.QueueUserWorkItem(new Action(() =>
                                {
                                    try
                                    {
                                        if (!Core.movieList.ContainsKey(Name)) return;
                                        string str1 = Name;
                                        string str2 = movie.id;
                                        string gid = Core.movieInfoList[str1][str2].movieInfo.Gid;
                                        string uc = Core.movieInfoList[str1][str2].movieInfo.Uc;

                                        Core.movieInfoList[str1][str2].magnets = OpenAPI.GetMovieMagnets(Settings.API, str2, gid, uc , Settings.SortingMode ? "size" : "date");
                                        if (Core.movieInfoList[str1][str2].magnets != null)
                                        {
                                            Core.movieList[str1][str2].state = 2;
                                        }
                                        Core.movieList[str1][str2].inSearch = false;
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.Message);
                                    }
                                }), 15);
                            }
                        }
                    }

                    if (Core.movieList[Name].Count < Settings.AutoloadingNum && HasNextPage && !inSearchNextPage )
                    {
                        inSearchNextPage = true;
                        Core.threadPool.QueueUserWorkItem(new Action(() =>
                        {
                            try
                            {
                                if (!Core.movieList.ContainsKey(Name)) return;
                                Core.SearchMovies(Name);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            inSearchNextPage = false;
                        }), 20);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                new Task(() =>
                {
                    KeyValuePair<string, Movies>[] array = Core.movieList[Name].ToArray();
                    MoviesJson = JsonConvert.SerializeObject(array, Converter.Settings);
                }).Start();

                Thread.Sleep(1000);
            }
        }

    }
}
