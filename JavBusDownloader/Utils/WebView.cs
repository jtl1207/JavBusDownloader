using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Net;
using System.Runtime.InteropServices;
using Data;

namespace JavBusDownloader
{
    public class WebView
    {
        public enum MessageBoxType
        {
            Info,
            Success,
            Error,
            Warning,
            Loading,
        }

        public class WebViewMain(WebView2 view, object obj_in = null)
        {
            bool isEnsemble = false;
            public WebView2 webview = view;
            public object obj = obj_in;

            bool IsWebview2Readly()
            {
                string res = "";
                try
                {
                    res = CoreWebView2Environment.GetAvailableBrowserVersionString();
                }
                catch (System.Exception)
                {
                }
                if (res == "" || res == null)
                {
                    return false;
                }
                return true;
            }

            public async Task LoadWebview()
            {
                if (Directory.Exists("WebView"))
                {
                    isEnsemble = true;
                }
                else
                {
                    if (!IsWebview2Readly())
                    {
                        string MicrosoftEdgeWebview2Setup = System.IO.Path.Combine(Application.StartupPath, "MicrosoftEdgeWebview2Setup.exe");

                        if (File.Exists(MicrosoftEdgeWebview2Setup))
                        {
                            Process.Start(MicrosoftEdgeWebview2Setup, " /silent /install").WaitForExit();
                            if (IsWebview2Readly())
                            {
                                Application.Restart();
                                Process.GetCurrentProcess()?.Kill();
                            }
                        }
                        else
                        {
                            await Task.Run(async () =>
                            {
                                using var webClient = new WebClient();
                                bool isDownload = false;
                                webClient.DownloadFileCompleted += (s, e) => { isDownload = true; };
                                await webClient.DownloadFileTaskAsync("https://go.microsoft.com/fwlink/p/?LinkId=2124703", "MicrosoftEdgeWebview2Setup.exe");

                                if (isDownload)
                                {
                                    Process.Start(MicrosoftEdgeWebview2Setup, " /silent /install").WaitForExit();
                                    if (IsWebview2Readly())
                                    {
                                        Application.Restart();
                                        Process.GetCurrentProcess()?.Kill();
                                    }
                                }
                            });
                            MessageBox.Show("未检测到WebView2Runtime，正在自动下载");
                        }
                    }
                }
            }

            public async Task<bool> Start(string location, string navigation, bool debug = false)
            {
                var result = await CoreWebView2Environment.CreateAsync(isEnsemble ? $"{AppDomain.CurrentDomain.BaseDirectory}\\WebView" : null, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cache"), options: new CoreWebView2EnvironmentOptions($"-disable-web-security --user-data-dir={AppDomain.CurrentDomain.BaseDirectory}\\ChromeDevSession --enable-features=msWebView2EnableDraggableRegions"));
                await webview.EnsureCoreWebView2Async(result);
                webview.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = false;
                if (obj != null)
                {
                    webview.CoreWebView2.AddHostObjectToScript("webBrowserObj", obj);
                    await webview.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("var webBrowserObj= window.chrome.webview.hostObjects.webBrowserObj;");
                    //await webview.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.addEventListener('DOMContentLoaded', () => {document.body.addEventListener('mousedown', evt => {const {target} = evt;const appRegion = getComputedStyle(target)['-webkit-app-region'];if (appRegion === 'drag') { webview.MouseDownDrag(); evt.preventDefault(); evt.stopPropagation();}});document.body.addEventListener('pointerdown', evt => {const {target} = evt;const appRegion = getComputedStyle(target)['-webkit-app-region'];if (appRegion === 'drag') { webview.MouseDownDrag(); evt.preventDefault(); evt.stopPropagation();}});});");
                }
                webview.CoreWebView2.SetVirtualHostNameToFolderMapping("sljly.xyz", location, CoreWebView2HostResourceAccessKind.Allow);
                webview.CoreWebView2.Navigate(navigation);
                webview.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
                if (debug)
                    DebugRunTime(webview);
                else
                    ProductionRunTimeAsync(webview);
                return true;
            }

            public void AddTrigger(WebView2 webView, EventHandler<CoreWebView2WebResourceResponseReceivedEventArgs> eventHandler)
            {
                webView.CoreWebView2.WebResourceResponseReceived += eventHandler;
            }

            private void webView21_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
            {
            }
            private void CoreWebView2_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e)
            {
                e.Handled = true;
            }
            private void DebugRunTime(WebView2 webView)
            {
            }
            private async Task ProductionRunTimeAsync(WebView2 webView)
            {
                webView.CoreWebView2.Settings.AreDevToolsEnabled = false;
                webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
                webView.CoreWebView2.Settings.IsZoomControlEnabled = false;
            }
        }

        [ClassInterface(ClassInterfaceType.AutoDual)]
        [ComVisible(true)]
        public class WebViewObj(Form form, WebView2 webview)
        {
            public void StartAsync(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
            {
                //webview.CoreWebView2.ExecuteScriptAsync($"LoadGoods({ConvertExpand.ToJson(Server.State.pricingSkuids)})");
            }

            public void SendMessage(MessageBoxType messageType, string str)
            {
                switch (messageType)
                {
                    case MessageBoxType.Info:
                        webview.CoreWebView2.ExecuteScriptAsync($"Dreamer.info(\"{str}\");");
                        break;
                    case MessageBoxType.Error:
                        webview.CoreWebView2.ExecuteScriptAsync($"Dreamer.error(\"{str}\");");
                        break;
                    case MessageBoxType.Warning:
                        webview.CoreWebView2.ExecuteScriptAsync($"Dreamer.warning(\"{str}\");");
                        break;
                    case MessageBoxType.Success:
                        webview.CoreWebView2.ExecuteScriptAsync($"Dreamer.success(\"{str}\");");
                        break;
                    case MessageBoxType.Loading:
                        webview.CoreWebView2.ExecuteScriptAsync($"Dreamer.loading(\"{str}\");");
                        break;
                }
            }

            public string GetMoviesList()
            {
                return ((Main)form).MoviesJson;
            }

            public void SaveApi(string url)
            {
                Settings.API = url;
            }
            public void Close()
            {
                form.Close();
            }
            public string GetMoviesMod()
            {
                return ((Main)form).type == MovieMod.normal ? "normal" : "uncensored";
            }

            private object lockObject = new object();

            public void DownloadMovie(string str)
            {

                try
                {
                    string outMagnets = $"{str}\r\n";
                    bool outText = false;
                    string outName = DateTime.Today.ToString("yyyy-MM-dd") + " " + form.Text + " " + form.Name;

                    if (Core.movieInfoList[form.Name][str].magnets != null)
                    {
                        outText = true;
                        for (int i = 0; i < Core.movieInfoList[form.Name][str].magnets.Length; i++)
                        {
                            if (i < Settings.DownloadNum)
                            {
                                outMagnets += $"{Core.movieInfoList[form.Name][str].magnets[i].ShareDate}  ";
                                outMagnets += $"{Core.movieInfoList[form.Name][str].magnets[i].Size}\r\n";
                                outMagnets += $"{Core.movieInfoList[form.Name][str].magnets[i].Link}\r\n";
                            }
                            else
                                break;
                        }
                    }
                    else
                    {
                        webview.CoreWebView2.ExecuteScriptAsync($"Dreamer.info(\"当前影片未获取全部数据,请稍后\");");
                    } 

                    if (outText)
                    {
                        webview.CoreWebView2.ExecuteScriptAsync($"Dreamer.success(\"种子获取成功\");");

                        //推送qt
                        if (Settings.QT != "")
                        {
                            if (!Core.PushQt(outMagnets))
                                webview.CoreWebView2.ExecuteScriptAsync($"Dreamer.success(\"magnet推送失败\");");
                        }
                        //推送本地文件
                        if (Settings.OutText)
                        {
                            Core.WriteData(outName, outMagnets);
                        }
                        //推送剪切板
                        if (Settings.OutShearPlate)
                        {
                            Clipboard.SetText(outMagnets);
                        }
                        //推送窗口
                        if (Settings.OutWindows)
                        {
                            Core.ShowData(outName, outMagnets);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    webview.CoreWebView2.ExecuteScriptAsync($"Dreamer.error(\"异常,请查看debug\");");
                }
            }
            public void DownloadAll()
            {
                try
                {
                    string outMagnets = "";
                    bool outText = false;
                    string outName = DateTime.Today.ToString("yyyy-MM-dd") + " " + form.Text + " " + form.Name;

                    int j = 0, k = 0;
                    foreach (var MoviesInfoList in Core.movieInfoList[form.Name])
                    {
                        j++;
                        if (MoviesInfoList.Value.magnets != null)
                        {
                            k++;
                            outText = true;
                            for (int i = 0; i < MoviesInfoList.Value.magnets.Length; i++)
                            {
                                if (i < Settings.DownloadNum)
                                {
                                    outMagnets += $"{MoviesInfoList.Value.magnets[i].ShareDate}  ";
                                    outMagnets += $"{MoviesInfoList.Value.magnets[i].Size}\r\n";
                                    outMagnets += $"{MoviesInfoList.Value.magnets[i].Link}\r\n";
                                }
                                else
                                    break;
                            }
                        }
                    }

                    if (outText)
                    {
                        webview.CoreWebView2.ExecuteScriptAsync($"Dreamer.success(\"种子获取成功  下载成功{k}  未完成{j}\");");

                        //推送qt
                        if (Settings.QT != "")
                        {
                            if (!Core.PushQt(outMagnets))
                                webview.CoreWebView2.ExecuteScriptAsync($"Dreamer.success(\"magnet推送失败\");");
                        }
                        //推送本地文件
                        if (Settings.OutText)
                        {
                            Core.NewWriteData(outName, outMagnets);
                        }
                        //推送剪切板
                        if (Settings.OutShearPlate)
                        {
                            Clipboard.SetText(outMagnets);
                        }
                        //推送窗口
                        if (Settings.OutWindows)
                        {
                            Core.ShowData(outName, outMagnets);
                        }
                    }
                    else
                    {
                        webview.CoreWebView2.ExecuteScriptAsync($"Dreamer.error(\"未发现可下载影片\");");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    webview.CoreWebView2.ExecuteScriptAsync($"Dreamer.error(\"异常,请查看debug\");");
                }
            }

            public void SearchMovies(string str)
            {
                ((Main)form).searchMod = SearchMod.search;
                ((Main)form).search = str;
                Core.RefreshForm(form.Name);
            }

            public void Min()
            {
                form.WindowState = FormWindowState.Minimized;
            }

            public void NewFormType()
            {
                Core.NewForm(((Main)form).type == MovieMod.normal ? MovieMod.uncensored : MovieMod.normal, SearchMod.search, "");
            }


            bool isNextRuning = false;
            public void NextPage()
            {
                if (isNextRuning) return;
                isNextRuning = true;

                if (((Main)form).HasNextPage)
                {
                    Core.SearchMovies(form.Name);
                }
                else
                {
                    webview.CoreWebView2.ExecuteScriptAsync("HasNextPage = false;Dreamer.success('目录加载完成');");
                }
                isNextRuning = false;
            }


            public void ShowSettingForm()
            {
                Setting form = new Setting();
                form.ShowDialog();
            }
            public void ShowGenreForm(string str)
            {
                Genre form = new Genre(str);
                form.Show();
            }
            public void OpenGithub()
            {
                Process.Start("https://github.com/jtl1207/JavBusDownloader");
            }

        }
    }
}
