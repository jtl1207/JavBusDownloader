using Bunifu.UI.WinForms;
using Data;
using System;
using System.Windows.Forms;

namespace JavBusDownloader
{
    public partial class Main : Form
    {
        string[] args = null;
        public Main(string name)
        {
            InitializeComponent();
            Name = name;
        }
        public Main(string name ,string[] args)
        {
            InitializeComponent();
            Name = name;
            this.args = args;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            ApiMovies mv = WebAPI.GetMovies("https://javbus-api-jtl1207.vercel.app/api/movies");
            Console.WriteLine();
        }
    }
}
