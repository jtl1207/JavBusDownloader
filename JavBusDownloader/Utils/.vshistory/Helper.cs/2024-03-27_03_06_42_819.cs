using System;
using System.Collections.Generic;
using System.Threading;
using Data;

namespace JavBusDownloader
{
    class Movies
    {
        int state = 0; // -1：失败 0：加载数据 1：加载种子 1：重试中 2：加载成功
        string date = "2024-04-27";
        string id = "BDST-012";
        string img = "https://www.javbus.com/pics/thumb/aerq.jpg";
        string[] tags = { };
        string title = "完全引退 AV女優、最後の1日。三上悠亜ラストセックス";
        ApiMovieInfo movieInfo = null;
        ApiMagnets[] magnets = null;
    }

    class Job
    {

    }

    class ThreadPool
    {
        enum ThreadPriority
        {
            normal,
            high,
            low
        }

        static Queue<WaitCallback> highPriorityWorkList = new Queue<WaitCallback>();
        static Queue<WaitCallback> normalPriorityWorkList = new Queue<WaitCallback>();
        static Queue<WaitCallback> lowPriorityWorkList = new Queue<WaitCallback>();

        static void QueueUserWorkItem(WaitCallback callBack , ThreadPriority priority = ThreadPriority.normal)
        {
            switch (priority)
            {
                                case ThreadPriority.high:
                    highPriorityWorkList.Enqueue(callBack);
                    break;
                case ThreadPriority.normal:
                    normalPriorityWorkList.Enqueue(callBack);
                    break;
                case ThreadPriority.low:
                    lowPriorityWorkList.Enqueue(callBack);
                    break;
            }
            //StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
            //return QueueUserWorkItemHelper(callBack, state, ref stackMark, compressStack: true);
        }
    }



    static class Core
    {
        static SortedList<string, Movies> movieList = new SortedList<string, Movies>();
        static SortedList<string, ThreadPool> threadPool = new SortedList<string, ThreadPool>();


        public static void NewSearchMovies(string mod = "" )
        {
            ThreadPool.QueueUserWorkItem(NewSearchMovies("1"));
        }




    }




}
