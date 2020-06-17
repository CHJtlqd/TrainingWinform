using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainMenuApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void MnuNewFile_Click(object sender, EventArgs e)
        {
            textBox1.Text += MnuNewFile.Text + Environment.NewLine;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void 열기OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text += 열기OToolStripMenuItem.Text + Environment.NewLine;

        }

        private void 닫기CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text += 닫기CToolStripMenuItem.Text + Environment.NewLine;

        }

        private void 저장SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text += 저장SToolStripMenuItem.Text + Environment.NewLine;
            MessageBox.Show("저장했습니다");

        }

        private void 종료XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();     // 프로그램 종료

        }

        private void 잘라내기TToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 복사CToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 붙여넣기PToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 프로그램정보AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox(); //기본으로 어셈블리 정보를 보여줌
            aboutBox.ShowDialog();
        }

        private void 모두선택AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(e.Location);
            }
        }

        private void textBox1_MouseMove(object sender, MouseEventArgs e)
        {
            LblMouseLocation.Text = e.Location.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = MnuNewFile.Text;
            toolStripComboBox1.Items.Add("Python");
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            button1.Focus();

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            MnuNewFile_Click(sender, e);
        }
    }
}
