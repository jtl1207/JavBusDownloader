using System.Linq;

namespace JavBusDownloader
{
    internal class Settings
    {
        //API接口
        //qt接口
        //单选模式时点击影片 下载/弹出详情
        //种子排序模式 时间/大小	
        //冗余下载数量		1/2/3
        //输出至文本文件
        //输出至文本
        //显示控制台
        private static class Data
        {
            static string[] API = Properties.Settings.Default.API.Split('|');
            static string QT = Properties.Settings.Default.QT;
            static bool RadioMode = Properties.Settings.Default.RadioMode;
            static bool SortingMode = Properties.Settings.Default.SortingMode;
            static int DownloadNumber = Properties.Settings.Default.DownloadNumber;
            static bool OutText = Properties.Settings.Default.OutText;
            static bool OutWindows = Properties.Settings.Default.OutWindows;
            static bool Debus = Properties.Settings.Default.Debus;
        }

        public static string[] API => Data.API;

    }
}
