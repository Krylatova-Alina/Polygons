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
		/*Vertex shape;*/
		List<Vertex> shapes = new List<Vertex>();
		int flagVertex = 0;
		bool Draw = false;
		List<int> delsX = new List<int>();
		List<int> delsY = new List<int>();
		/*int delX, delY;*/
		bool DrawDrag;
		List<bool> DrawDrags = new List<bool>();
		public Form1()
		{
			InitializeComponent();
			/*delX = delY = 0;*/
			DrawDrag = false;
		}

		private void Form1_Paint(object sender, PaintEventArgs e)
		{
			if (Draw)
				foreach (Vertex shape in shapes)
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

		private void Form1_MouseDown(object sender, MouseEventArgs e)
		{
			
			if (e.Button == MouseButtons.Left)
				{
					foreach (Vertex shape in shapes)
						if (shape.Check(e.X, e.Y))
						{
							DrawDrag = true;
							DrawDrags.Add(true);
							delsX.Add(e.X - shape.SetX);
							delsY.Add(e.Y - shape.SetY);
						}
						else
						{
							DrawDrags.Add(false);
							delsX.Add(0);
							delsY.Add(0);
						}

					if (!DrawDrag)
					{
						if (!Draw)
							Draw = true;
						switch (flagVertex)
						{
							case 0: shapes.Add(new Circle(e.X, e.Y)); break;
							case 1: shapes.Add(new Rectangle(e.X, e.Y)); break;
							case 2: shapes.Add(new Triangle(e.X, e.Y)); break;
						}
					}					
				}
			if (e.Button == MouseButtons.Right)
				{
					for (int i = shapes.Count - 1; i >= 0; i--)
						if (shapes[i].Check(e.X, e.Y))
						{
							shapes.RemoveAt(i);
						}
					if (shapes.Count == 0)
						Draw = false;
				}			
			this.Refresh();
		}   

		private void Form1_MouseMove(object sender, MouseEventArgs e)
		{
			if (DrawDrag/*DrawDrags.Contains(true)*/)
			{
				for (int i = 0; i < shapes.Count; i++)                   
					if (DrawDrags[i])
					{
						shapes[i].SetX = e.X - delsX[i];
						shapes[i].SetY = e.Y - delsY[i];
					}
			}
			this.Refresh();
		}

		private void Form1_MouseUp(object sender, MouseEventArgs e)
		{
			DrawDrag = false;
			DrawDrags.Clear();
			delsX.Clear();
			delsY.Clear();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			DoubleBuffered = true;
		}

			
	}
}
