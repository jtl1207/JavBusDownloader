using Data;
using System;
using System.Windows.Forms;
using WebApi;

namespace JavBusDownloader
{
    public partial class Main : Form
    {
        string[] args = null;
        public Main()
        {
            InitializeComponent();
        }
        public Main(string[] args)
        {
            InitializeComponent();
            this.args = args;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            ApiMovies mv = WebAPI.GetMovies("https://javbus-api-jtl1207.vercel.app/api/movies");
            Console.WriteLine();
        }
    }
}
