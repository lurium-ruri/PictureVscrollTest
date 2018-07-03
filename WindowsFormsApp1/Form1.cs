using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private static readonly int PICTURE_WIDTH = 678;
        private static readonly int PICTURE_HEIGHT = 1080;
        public Form1()
        {
            InitializeComponent();

            //picture読み込み
            string[] files = System.IO.Directory.GetFiles(@".\img", "*");
            Array.Sort(files, new NaturalStringCompare());
            var height = 0;
            foreach (string filePath in files)
            {
                var pictureBox = new PictureBox
                {
                    Image = new Bitmap(filePath),
                    Location = new System.Drawing.Point(0, 0),
                    Name = "pictureBox1",
                    Size = new System.Drawing.Size(PICTURE_WIDTH, PICTURE_HEIGHT),
                    Margin = new Padding(0, 0, 0, 0)
                };
                this.flowLayoutPanel1.Controls.Add(pictureBox);
                height += PICTURE_HEIGHT;
            }
            var width = flowLayoutPanel1.Size.Width;
            this.flowLayoutPanel1.Size = new System.Drawing.Size(width, height);

            //スクロールバーの設定
            //最大値にはプログラムによってしか到達できません。
            //ユーザーとの対話型操作によって到達できる最大値は、1 に Maximum プロパティ値を加えてから
            //LargeChange プロパティ値を引いた値と同じです。
            this.vScrollBar1.Maximum = height - 1;
            this.vScrollBar1.LargeChange = this.panel2.Height;
            this.vScrollBar1.SmallChange = this.panel2.Height;
        }


        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            int x = this.flowLayoutPanel1.Location.X;
            int y = e.NewValue;

            this.flowLayoutPanel1.Location = new Point(x, -y);
            this.flowLayoutPanel1.Refresh();
        }
    }

    class NaturalStringCompare : IComparer<string>
    {
        int IComparer<string>.Compare(string x, string y)
        {
            // 名称の比較
            return NaturalCompare(x, y);
        }

        private int NaturalCompare(string x, string y)
        {
            if (x == y)
            {
                return 0;
            }

            var ix = 0;
            var iy = 0;

            if (int.TryParse(x, out ix) && int.TryParse(y, out iy))
            {
                return ix.CompareTo(iy);
            }

            var xs = Regex.Split(x.Replace(" ", ""), @"(\d+)");
            var ys = Regex.Split(y.Replace(" ", ""), @"(\d+)");

            if (xs.Length == 1 && ys.Length == 1)
            {
                return x.CompareTo(y);
            }

            var minLength = Math.Min(xs.Length, ys.Length);

            foreach (var i in Enumerable.Range(0, minLength))
            {
                if (xs[i] != ys[i])
                {
                    return NaturalCompare(xs[i], ys[i]);
                }
            }

            return xs.Length.CompareTo(ys.Length);
        }
    }
}
