using Bunifu.UI.WinForms;
using RestSharp;
using System;
using System.Drawing;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace JavBusDownloader
{
    public partial class Setting : Form
    {
        bool ready = false;
        public Setting()
        {
            InitializeComponent();

            textBox1.Text = Settings.API;

            string[] strings = Settings.QT.Split('|');
            if (strings.Length == 3)
            {
                textBox2.Text = strings[0];
                textBox4.Text = strings[1];
                textBox5.Text = strings[2];
            }
            textBox3.Text = Settings.AutoloadingNum.ToString();
            bunifuRadioButton2.Checked = Settings.SortingMode;
            bunifuRadioButton1.Checked = !Settings.SortingMode;
            bunifuRadioButton6.Checked = Settings.DownloadNum == 1;
            bunifuRadioButton4.Checked = Settings.DownloadNum == 2;
            bunifuRadioButton5.Checked = Settings.DownloadNum == 3;
            bunifuCheckBox1.Checked = Settings.OutText;
            bunifuCheckBox2.Checked = Settings.OutShearPlate;
            bunifuCheckBox3.Checked = Settings.OutWindows;
            bunifuRadioButton9.Checked = Settings.Debug;
            bunifuRadioButton10.Checked = !Settings.Debug;
            ready = true;
        }

        private void bunifuButton24_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuRadioButton1_CheckedChanged2(object sender, Bunifu.UI.WinForms.BunifuRadioButton.CheckedChangedEventArgs e)
        {
            if (e.Checked & ready)
            {
                Settings.SortingMode = false;
            }
        }

        private void bunifuRadioButton2_CheckedChanged2(object sender, Bunifu.UI.WinForms.BunifuRadioButton.CheckedChangedEventArgs e)
        {
            if (e.Checked & ready)
            {
                Settings.SortingMode = true;
            }
        }

        private void bunifuRadioButton3_CheckedChanged2(object sender, Bunifu.UI.WinForms.BunifuRadioButton.CheckedChangedEventArgs e)
        {
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Settings.AutoloadingNum = int.Parse(textBox3.Text != "" ? textBox3.Text : "0" );
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void bunifuRadioButton6_CheckedChanged2(object sender, Bunifu.UI.WinForms.BunifuRadioButton.CheckedChangedEventArgs e)
        {
            if (e.Checked & ready)
            {
                Settings.DownloadNum = 1;
            }
        }

        private void bunifuRadioButton4_CheckedChanged2(object sender, Bunifu.UI.WinForms.BunifuRadioButton.CheckedChangedEventArgs e)
        {
            if (e.Checked & ready)
            {
                Settings.DownloadNum = 2;
            }
        }
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();
        private void bunifuRadioButton9_CheckedChanged2(object sender, Bunifu.UI.WinForms.BunifuRadioButton.CheckedChangedEventArgs e)
        {
            if (e.Checked & ready)
            {
                Settings.Debug = true;
                AllocConsole(); 
            }
        }

        private void bunifuRadioButton10_CheckedChanged2(object sender, Bunifu.UI.WinForms.BunifuRadioButton.CheckedChangedEventArgs e)
        {
            if (e.Checked & ready)
            {
                Settings.Debug = false;
                FreeConsole();
            }
        }

        private void bunifuRadioButton5_CheckedChanged2(object sender, Bunifu.UI.WinForms.BunifuRadioButton.CheckedChangedEventArgs e)
        {
            if (e.Checked & ready)
            {
                Settings.DownloadNum = 3;
            }
        }

        private void bunifuButton21_Click(object sender, EventArgs e)
        {
            double i = Core.CheckApi(textBox1.Text);
            if (i > 0)
            {
                MessageBox.Show($"API地址有效,延迟{i}ms");
                Settings.API = textBox1.Text;
            }
            else
            {
                MessageBox.Show($"API地址无效或网络异常");
            }
        }

        private async void bunifuButton22_Click(object sender, EventArgs e)
        {
            string qbittorrentUrl = textBox2.Text;
            string username = textBox4.Text;
            string password = textBox5.Text;
            try
            {
                string apiPath = "/api/v2/auth/login";

                var client = new RestClient(qbittorrentUrl);
                var request = new RestRequest(apiPath, Method.POST);

                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("username", username);
                request.AddParameter("password", password);
                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    MessageBox.Show("qBittorrent连接成功");
                    Settings.QT = $"{qbittorrentUrl}|{username}|{password}";
                    Core.CheckQt();
                }
                else
                {
                    MessageBox.Show("qBittorrent连接失败");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生异常：{ex.Message}");
            }
        }

        private void bunifuCheckBox1_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs e)
        {
            Settings.OutText = e.Checked;
        }

        private void bunifuCheckBox2_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs e)
        {
            Settings.OutShearPlate = e.Checked;
        }

        private void bunifuCheckBox3_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs e)
        {
            Settings.OutWindows = e.Checked;
        }
    }
}
