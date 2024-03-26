using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Data;
using QBittorrent.Client;
using RestSharp;
using static System.Net.Mime.MediaTypeNames;
using static JavBusDownloader.ThreadPool;
using static JavBusDownloader.WebView;

namespace JavBusDownloader
{
    public enum SearchMod
    {
        search,
        star,
        genre,
        director,
        studio,
        label,
        series,
    }
    public enum MovieMod
    {
        uncensored,
        normal,
    }
    public class Movies
    {
        public int state = 0; // 0：加载数据 1：加载种子 2：加载成功
        public bool inSearch = false;

        public string date = "";
        public string id = "";
        public string img = "";
        public string[] tags = { };
        public string title = "";

    }

    public class MoviesInfo
    {
        public ApiMovieInfo movieInfo = null;
        public ApiMagnets[] magnets = null;
    }

    public class ThreadPool(int maxThread)
    {
        public class PriorityQueue<TValue>
        {
            private SortedDictionary<int, Queue<TValue>> dict = new SortedDictionary<int, Queue<TValue>>(Comparer<int>.Create((x, y) => y.CompareTo(x)));

            public int Count { get; private set; }

            public void Enqueue(int priority, TValue value)
            {
                if (!dict.ContainsKey(priority))
                {
                    dict[priority] = new Queue<TValue>();
                }

                dict[priority].Enqueue(value);
                Count++;
            }

            public TValue Dequeue()
            {
                TValue result = default;
                if (Count > 0)
                {
                    var first = dict.First();
                    int priority = first.Key;
                    TValue value = first.Value.Dequeue();
                    if (first.Value.Count == 0)
                    {
                        dict.Remove(priority);
                    }
                    Count--;
                    result = value;
                }
                return result;
            }
        }
        private PriorityQueue<Action> priorityQueue = new PriorityQueue<Action>();

        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread);
        bool inWork = false;
        int waitTime = 100;
        public int Count
        {
            get
            {
                return priorityQueue.Count;
            }
        }
        public void QueueUserWorkItem(Action task, int ThreadPriority = 10)
        {
            priorityQueue.Enqueue(ThreadPriority, task);
            DoWork();
        }

        async Task DoWork()

        {
            if (inWork) return;
            inWork = true;

            while (priorityQueue.Count > 0)
            {
                await semaphore.WaitAsync();
                Action pair = priorityQueue.Dequeue();
                Task.Run(() =>
                {
                    try
                    {
                        pair();
                        Thread.Sleep(waitTime);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });
                Thread.Sleep(0);
            }
            inWork = false;
        }
    }

    static class Core
    {
        public static SortedList<string, Main> fromList = new SortedList<string, Main>();
        public static SortedList<string, SortedList<string, Movies>> movieList = new SortedList<string, SortedList<string, Movies>>();
        public static SortedList<string, SortedList<string, MoviesInfo>> movieInfoList = new SortedList<string, SortedList<string, MoviesInfo>>();
        public static ThreadPool threadPool;


        // 初始化
        public static void Initialize()
        {
            if (CheckApi(Settings.API) <= 0)
                MessageBox.Show("接口测试异常，请检查网络连接或设置API接口");
            CheckQt();
        }

        public static void NewForm(MovieMod movieMod, SearchMod searchMod, string str, string anothername = "")
        {
            string name = new Random().Next(100000).ToString();
            fromList.Add(name, new Main() { Name = name });
            movieList.Add(name, new SortedList<string, Movies>());
            movieInfoList.Add(name, new SortedList<string, MoviesInfo>());
            fromList[name].searchMod = searchMod;
            fromList[name].type = movieMod;
            fromList[name].search = str;
            fromList[name].Text = $"{(movieMod == MovieMod.normal ? "有码" : "无码")}  {(anothername == "" ? str : anothername)}";
            NewSearchMovies(name);
            fromList[name].Show();
        }
        public static void RefreshForm(string fromname)
        {
            movieList[fromname].Clear();
            movieInfoList[fromname].Clear();

            NewSearchMovies(fromname);
        }
        public static void CloseForm(string fromname)
        {
            fromList.Remove(fromname);
            movieList.Remove(fromname);
            movieInfoList.Remove(fromname);
        }

        #region 写入文件
        public static bool WriteData(string name, string value)
        {
            try
            {
                if (!Directory.Exists("Download"))
                {
                    Directory.CreateDirectory("Download");
                }

                using (StreamWriter writer = new StreamWriter($"Download/{name}.txt", true))
                {
                    writer.WriteLine(value);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"写入文件时出错：{ex.Message}");
            }
            return false;
        }
        public static bool NewWriteData(string name, string value)
        {
            try
            {
                string filePath = $"Download/{name}.txt";

                if (!Directory.Exists("Download"))
                {
                    Directory.CreateDirectory("Download");
                }

                File.WriteAllText(filePath, value);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"写入文件时出错：{ex.Message}");
            }
            return false;

        }
        public static void ShowData(string name,string str)
        {
            TextFrom textFrom = new TextFrom();
            textFrom.newText(name, str);
            textFrom.ShowDialog();
            textFrom.Dispose();
        }
        #endregion
        #region QT
        public static QBittorrentClient QtClient = null;

        public static async void CheckQt()
        {
            string[] strings = Settings.QT.Split('|');
            if (strings.Length == 3)
            {
                try
                {
                    QtClient = new QBittorrentClient(new Uri(strings[0]));
                    await QtClient.LoginAsync(strings[1], strings[2]);
                }
                catch
                {
                    MessageBox.Show("qBittorrent连接异常，请检查网络连接或设置API接口");
                }
            }
        }
        public static bool PushQt(string magnet)
        {
            if (QtClient != null)
            {
                try
                {
                    string[] lines = magnet.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    string pattern = @"magnet:\?xt=urn:btih:[\w\d]+";

                    List<Uri> torrents = new List<Uri>();

                    foreach (string line in lines)
                    {
                        Match match = Regex.Match(line, pattern);
                        if (match.Success)
                        {
                            torrents.Add(new Uri(match.Value));
                        }
                    }

                    AddTorrentUrlsRequest addTorrentsRequest = new(torrents);
                    QtClient.AddTorrentsAsync(addTorrentsRequest);
                    return true;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return false;
        }
        #endregion
        #region 检查接口
        public static double CheckApi(string url)
        {
            if (string.IsNullOrEmpty(url) | url == "") return -1;
            DateTime dateTime = DateTime.Now;
            for (int i = 0; i <= 3; i++)
            {
                try
                {
                    ApiMovies mv = OpenAPI.GetMoviesAsync(url);
                    if (mv != null)
                    {
                        threadPool = new ThreadPool(1);
                        return (DateTime.Now - dateTime).TotalSeconds == 0 ? -1 : (DateTime.Now - dateTime).TotalSeconds;
                    }
                }
                catch { }
            }
            return -1;
        }
        #endregion

        public static void NewSearchMovies(string fromName)
        {
            if (threadPool == null)
            {
                return;
            }
            fromList[fromName].page = 0;
            fromList[fromName].HasNextPage = true;

            movieList[fromName].Clear();
            movieInfoList[fromName].Clear();

            threadPool.QueueUserWorkItem(new Action(() =>
            {
                SearchMovies(fromName);
            }), 20);
        }

        public static void SearchMovies(string fromName)
        {
            if (!fromList[fromName].HasNextPage) return;
            ApiMovies movies = new ApiMovies();
            int i = 0;
            while (i < 4)
            {
                if (!Core.movieList.ContainsKey(fromList[fromName].Name)) return;
                i++;
                if (fromList[fromName].search == "")
                {
                    movies = OpenAPI.GetMoviesAsync(Settings.API, fromList[fromName].page + 1, type: fromList[fromName].type == MovieMod.uncensored ? "uncensored" : "normal");
                }
                else
                {
                    switch (fromList[fromName].searchMod)
                    {
                        case SearchMod.search:
                            movies = OpenAPI.SearchMovies(Settings.API, fromList[fromName].search, fromList[fromName].page + 1, type: fromList[fromName].type == MovieMod.uncensored ? "uncensored" : "normal");
                            break;
                        case SearchMod.genre:
                            movies = OpenAPI.GetMoviesAsync(Settings.API, fromList[fromName].page + 1, "genre", fromList[fromName].search, type: fromList[fromName].type == MovieMod.uncensored ? "uncensored" : "normal");
                            break;
                        case SearchMod.series:
                            movies = OpenAPI.GetMoviesAsync(Settings.API, fromList[fromName].page + 1, "series", fromList[fromName].search, fromList[fromName].type == MovieMod.uncensored ? "uncensored" : "normal");
                            break;
                        case SearchMod.label:
                            movies = OpenAPI.GetMoviesAsync(Settings.API, fromList[fromName].page + 1, "label", fromList[fromName].search, fromList[fromName].type == MovieMod.uncensored ? "uncensored" : "normal");
                            break;
                        case SearchMod.studio:
                            movies = OpenAPI.GetMoviesAsync(Settings.API, fromList[fromName].page + 1, "studio", fromList[fromName].search, fromList[fromName].type == MovieMod.uncensored ? "uncensored" : "normal");
                            break;
                        case SearchMod.director:
                            movies = OpenAPI.GetMoviesAsync(Settings.API, fromList[fromName].page + 1, "director", fromList[fromName].search, fromList[fromName].type == MovieMod.uncensored ? "uncensored" : "normal");
                            break;
                        case SearchMod.star:
                            movies = OpenAPI.GetMoviesAsync(Settings.API, fromList[fromName].page + 1, "star", fromList[fromName].search, fromList[fromName].type == MovieMod.uncensored ? "uncensored" : "normal");
                            break;
                    }
                }
                if (movies == null)
                {
                    Thread.Sleep(200 * i);
                    continue;
                }
                else
                {
                    break;
                }
            }
            if (i > 3)
            {
                ((WebViewObj)fromList[fromName].webviewMain.obj).SendMessage(MessageBoxType.Error, "网络异常");
                return;
            }
            fromList[fromName].HasNextPage = (bool)movies.Pagination.HasNextPage;
            if (movies.Movies.Length == 0 && !(bool)movies.Pagination.HasNextPage)
            {
                ((WebViewObj)fromList[fromName].webviewMain.obj).SendMessage(MessageBoxType.Error, "未找到搜索结果");
                return;
            }
            if (movies.Movies.Length != 0)
            {
                fromList[fromName].page++;
                foreach (Movie movie in movies.Movies)
                {
                    movieList[fromName].Add(movie.Id, new Movies
                    {
                        date = movie.Date,
                        id = movie.Id,
                        img = movie.Img,
                        tags = movie.Tags,
                        title = movie.Title,
                        state = 0
                    });
                    movieInfoList[fromName].Add(movie.Id, new MoviesInfo());
                }
                return;
            }
        }

        public static void SearchMovieInfo(string formname, string moviename)
        {

        }

    }
}
