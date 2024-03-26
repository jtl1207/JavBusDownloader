using System.Linq;

namespace JavBusDownloader
{
    internal class Settings
    {

        private static class Data
        {
            static string[] API = Properties.Settings.Default.API.Split('|');//API接口
            static string QT = Properties.Settings.Default.QT;//qt接口
            static bool RadioMode = Properties.Settings.Default.RadioMode;//单选模式时点击影片 下载/弹出详情
            static bool SortingMode = Properties.Settings.Default.SortingMode;//种子排序模式 时间/大小
            static int DownloadNumber = Properties.Settings.Default.DownloadNumber;//冗余下载数量		1/2/3
            static bool OutText = Properties.Settings.Default.OutText;//输出至文本文件
            static bool OutWindows = Properties.Settings.Default.OutWindows;//输出至文本
            static bool Debus = Properties.Settings.Default.Debus;//显示控制台
        }



    }
}
