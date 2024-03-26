using System;
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
        //自动加载影片数量  100
        //输出至文本文件
        //输出至文本
        //显示控制台
        internal static class Data
        {
            internal static string[] API = Properties.Settings.Default.API.Split('|');
            internal static string QT = Properties.Settings.Default.QT;
            internal static bool RadioMode = Properties.Settings.Default.RadioMode;
            internal static bool SortingMode = Properties.Settings.Default.SortingMode;
            internal static byte DownloadNum = Properties.Settings.Default.DownloadNum;
            internal static byte AutoloadingNum = Properties.Settings.Default.AutoloadingNum;
            internal static bool OutText = Properties.Settings.Default.OutText;
            internal static bool OutWindows = Properties.Settings.Default.OutWindows;
            internal static bool Debus = Properties.Settings.Default.Debus;
        }

        public static string[] API
        {
            get
            {
                return Data.API;
            }
            set
            {
                Data.API= value;
                Properties.Settings.Default["API"] = String.Join("|", Data.API); ;
                Properties.Settings.Default.Save();
            }
        }
        public static string QT
        {
            get
            {
                return Data.QT;
            }
            set
            {
                Properties.Settings.Default["QT"] = Data.QT = value;
                Properties.Settings.Default.Save();
            }
        }
        public static bool RadioMode
        {
            get
            {
                return Data.RadioMode;
            }
            set
            {
                Properties.Settings.Default["RadioMode"] = Data.RadioMode = value;
                Properties.Settings.Default.Save();
            }
        }
        public static bool SortingMode
        {
            get
            {
                return Data.SortingMode;
            }
            set
            {
                Properties.Settings.Default["SortingMode"] = Data.SortingMode = value;
                Properties.Settings.Default.Save();
            }
        }
        public static bool OutText
        {
            get
            {
                return Data.OutText;
            }
            set
            {
                Properties.Settings.Default["OutText"] = Data.OutText = value;
                Properties.Settings.Default.Save();
            }
        }
        public static bool OutWindows
        {
            get
            {
                return Data.OutWindows;
            }
            set
            {
                Properties.Settings.Default["OutWindows"] = Data.OutWindows = value;
                Properties.Settings.Default.Save();
            }
        }
        public static bool Debus
        {
            get
            {
                return Data.Debus;
            }
            set
            {
                Properties.Settings.Default["Debus"] = Data.Debus = value;
                Properties.Settings.Default.Save();
            }
        }
        public static byte DownloadNumber
        {
            get
            {
                return Data.DownloadNumber;
            }
            set
            {
                Properties.Settings.Default["DownloadNumber"] = Data.DownloadNumber = value;
                Properties.Settings.Default.Save();
            }
        }


    }
}
