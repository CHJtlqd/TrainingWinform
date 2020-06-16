using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckBoxTestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //private void UpdateLabel1(string s, bool b)
        //{

        //    if (b == true)
        //    {
        //        label1.Text += s + " ";

        //    }
        //    else
        //    {
        //        label1.Text = label1.Text.Replace(s, "");
        //    }
        //}

        private void UpdateLabel1()
        {
            string strchk1 = "", strchk2 = "", strchk3 = "", strchk4 = "";
            if (checkBox1.Checked) strchk1 = checkBox1.Text;
            else strchk1 = ""; 
            if (checkBox2.Checked) strchk2 = checkBox2.Text;
            else strchk2 = ""; 
            if (checkBox3.Checked) strchk3 = checkBox3.Text;
            else strchk3 = ""; 
            if (checkBox4.Checked) strchk4 = checkBox4.Text;
            else strchk4 = "";

            label1.Text = strchk1 + " " + strchk2 + " " + strchk3 + " " + strchk4;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //UpdateLabel1(checkBox1.Text, checkBox1.Checked);
            UpdateLabel1();

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

            // UpdateLabel1(checkBox2.Text, checkBox2.Checked);
            UpdateLabel1();

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            // UpdateLabel1(checkBox3.Text, checkBox3.Checked);
            UpdateLabel1();

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

            //UpdateLabel1(checkBox4.Text, checkBox4.Checked);
            UpdateLabel1();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "좋아하는 과일 : ";
        }
    }
}
