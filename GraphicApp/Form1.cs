using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Form1_Paint, 폼에 이미지 그리기 메서드
        /// created date : 2020.06.17
        /// 소ㅑㄹ라소ㅑㄹ라
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = CreateGraphics();

            Pen pen = new Pen(Color.DeepPink);
            pen.Width = 6.8f;
            //pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            //Point startPoint = new Point(45, 45);
            //Point endPoint = new Point(180, 180);
            //g.DrawLine(pen, startPoint, endPoint);

            //Rectangle rect = new Rectangle(50, 50, 150, 100);
            Rectangle[] rects = new Rectangle[]
            {
                new Rectangle(40,40,40,100),
                new Rectangle(100,40,100,40),
            new Rectangle(100, 100, 100, 40)

        };
            g.FillRectangles(Brushes.BlueViolet, rects);
            g.DrawRectangles(pen, rects);

            

        }
    }
}
