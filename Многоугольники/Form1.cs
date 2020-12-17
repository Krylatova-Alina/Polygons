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
		List<Vertex> shapes = new List<Vertex>();
		int flagVertex = 0;
		bool Draw = false;
		bool DrawDrag;
		public Form1()
		{
			InitializeComponent();		
			DrawDrag = false;
		}

		private void Form1_Paint(object sender, PaintEventArgs e)
		{
			if (Draw)
				foreach (Vertex shape in shapes)
					shape.DrawFigure(e.Graphics);
			if (shapes.Count > 2)
			{
				Pen pen = new Pen(Vertex.col);
				for (int i = 0; i < shapes.Count; i++)
					shapes[i].IsInside = false;
				for (int i = 0; i < shapes.Count-1; i++)
				{
					for (int j = i + 1; j < shapes.Count; j++)
					{
						bool flagUp = false;
						bool flagDown = false;
						for (int k = 0; k < shapes.Count; k++)
						{ 
							if (k!=j && k!=i && i!=j)
							{
								int y;
								if ((shapes[j].SetX - shapes[i].SetX) != 0)
									y = (shapes[k].SetX - shapes[j].SetX) * (shapes[j].SetY - shapes[i].SetY) / (shapes[j].SetX - shapes[i].SetX) + shapes[j].SetY;
								else
									y = shapes[i].SetY;
								if (y < shapes[k].SetY)
									flagUp = true;
								if (y >= shapes[k].SetY)
									flagDown = true;
							}
						}
						if (flagUp ^ flagDown)
						{
							shapes[i].IsInside = true;
							shapes[j].IsInside = true;
							e.Graphics.DrawLine(pen, shapes[i].SetX, shapes[i].SetY, shapes[j].SetX, shapes[j].SetY);
						}
					}
				}
			}
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
							shape.DragFlag = true;
							shape.DelX = e.X - shape.SetX;
							shape.DelY = e.Y - shape.SetY;
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
			if (DrawDrag)
			{
				for (int i = 0; i < shapes.Count; i++)                   
					if (shapes[i].DragFlag)
					{
						shapes[i].SetX = e.X - shapes[i].DelX;
						shapes[i].SetY = e.Y - shapes[i].DelY;
					}
			}
			this.Refresh();
		}

		private void Form1_MouseUp(object sender, MouseEventArgs e)
		{
			DrawDrag = false;
			for (int i = 0; i < shapes.Count; i++)
				shapes[i].DragFlag = false;
			if (shapes.Count > 2)
				for (int i = 0; i < shapes.Count; i++)
				{
					if (!shapes[i].IsInside)
					{
						shapes.RemoveAt(i);
					}
				}
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			DoubleBuffered = true;
		}
	}
}