using System;
using System.Linq;

namespace JavBusDownloader
{
    internal class Settings
    {
        public static string API
        {
            get
            {
                return Properties.Settings.Default.API;
            }
            set
            {
                Properties.Settings.Default["API"] =  value;
                Properties.Settings.Default.Save();
            }
        } //API接口
        public static string QT
        {
            get
            {
                return Properties.Settings.Default.QT;
            }
            set
            {
                Properties.Settings.Default["QT"] = value;
                Properties.Settings.Default.Save();
            }
        }//QT接口
        public static bool RadioMode
        {
            get
            {
                return Properties.Settings.Default.RadioMode;
            }
            set
            {
                Properties.Settings.Default["RadioMode"] = value;
                Properties.Settings.Default.Save();
            }
        }//单选模式时点击影片 快速下载/弹出详情
        public static bool SortingMode
        {
            get
            {
                return Properties.Settings.Default.SortingMode;
            }
            set
            {
                Properties.Settings.Default["SortingMode"]  = value;
                Properties.Settings.Default.Save();
            }
        }//种子排序模式 时间/大小
        public static bool OutText
        {
            get
            {
                return Properties.Settings.Default.OutText;
            }
            set
            {
                Properties.Settings.Default["OutText"] = value;
                Properties.Settings.Default.Save();
            }
        } //输出至文本
        public static bool OutShearPlate
        {
            get
            {
                return Properties.Settings.Default.OutShearPlate;
            }
            set
            {
                Properties.Settings.Default["OutShearPlate"] =  value;
                Properties.Settings.Default.Save();
            }
        } //输出至剪切板
        public static bool OutWindows
        {
            get
            {
                return Properties.Settings.Default.OutWindows;
            }
            set
            {
                Properties.Settings.Default["OutWindows"] =  value;
                Properties.Settings.Default.Save();
            }
        } //输出至窗口
        public static bool Debug
        {
            get
            {
                return Properties.Settings.Default.Debug;
            }
            set
            {
                Properties.Settings.Default["Debug"]  = value;
                Properties.Settings.Default.Save();
            }
        }//显示控制台
        public static byte DownloadNum
        {
            get
            {
                return Properties.Settings.Default.DownloadNum;
            }
            set
            {
                Properties.Settings.Default["DownloadNum"] =  value;
                Properties.Settings.Default.Save();
            }
        }//默认下载种子数量
        public static int AutoloadingNum
        {
            get
            {
                return Properties.Settings.Default.AutoloadingNum;
            }
            set
            {
                Properties.Settings.Default["AutoloadingNum"] =  value;
                Properties.Settings.Default.Save();
            }
        }//自动加载影片数量


    }
}
