using System.Collections.Generic;
using Data;

namespace JavBusDownloader
{
    static class Data
    {
        static SortedList<string, Movies> data_movies = new SortedList<string, Movies>();



    }



    class Movies
    {
        int state = 0; // -1：失败 0：加载数据 1：加载种子 1：重试中 2：加载成功
        string date = "2024-04-27";
        string id = "BDST-012";
        string img = "https://www.javbus.com/pics/thumb/aerq.jpg";
        string[] tags = { };
        string title = "完全引退 AV女優、最後の1日。三上悠亜ラストセックス";
        MovieData movieData = null;
        Magnet[] magnets = { };
    }

}
