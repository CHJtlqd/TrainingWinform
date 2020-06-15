using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();      // Design의 컴포넌트를 뿌려줌
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void BtnMessage_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            // MessageBox.Show($"Hello World! {now}");

            TxtCurrentDate.Text = now.ToString();
        }

        
    }
}
