using Bunifu.UI.WinForms;
using Data;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace JavBusDownloader
{
    public partial class TextFrom : Form
    {
        public TextFrom()
        {
            InitializeComponent();
        }
        private void ConsoleFrom_Load(object sender, EventArgs e)
        {

        }

        private void bunifuButton24_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void newText(string name, string str)
        {
            label1.Text = name;
            textBox1.Text = str;
        }
    }
}
