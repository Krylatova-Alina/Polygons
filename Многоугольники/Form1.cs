using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Многоугольники
{
    public partial class Form1 : Form
    {
        Vertex shape;
        int flagVertex = 0;
        bool Draw = false;
        int delX, delY;
        bool DrawDrag;
        public Form1()
        {
            InitializeComponent();
            delX = delY = 0;
            DrawDrag = false;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (Draw)
                shape.DrawFigure(e.Graphics);
        }

        private void кругToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flagVertex = 0;
        }
        private void квадратToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flagVertex = 1;
        }
        private void треугольникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flagVertex = 2;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (DrawDrag)
            {
                shape.SetX = e.X - delX;
                shape.SetY = e.Y - delY;
            }
            this.Refresh();
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            DrawDrag = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (Draw && shape.Check(e.X, e.Y))
            {
                if (e.Button == MouseButtons.Left)
                {
                    delX = e.X - shape.SetX;
                    delY = e.Y - shape.SetY;
                    DrawDrag = true;
                }
                if (e.Button == MouseButtons.Right)
                {
                    Draw = false;
                    this.Refresh();
                }
            }
            else
            {
                Draw = true;
                switch (flagVertex)
                {
                    case 0: shape = new Circle(e.X, e.Y); break;
                    case 1: shape = new Rectangle(e.X, e.Y); break;
                    case 2: shape = new Triangle(e.X, e.Y); break;
                }                
            }
            this.Refresh();
        }       
    }
}
