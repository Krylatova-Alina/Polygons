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
		bool DrawDrag, DrawDragFigure;

		public Form1()
		{
			InitializeComponent();		
			DrawDrag = DrawDragFigure = false;
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
						bool flagUp = true;
						bool flagDown = true;
						for (int k = 0; k < shapes.Count; k++)
						{ 
							if (k!=j && k!=i && i!=j)
							{
								if ((shapes[j].SetX - shapes[i].SetX) != 0)
									if ((shapes[k].SetX - shapes[i].SetX) * (shapes[j].SetY - shapes[i].SetY) / (shapes[j].SetX - shapes[i].SetX) + shapes[i].SetY <= shapes[k].SetY)
										flagUp = false;
									else
										flagDown = false;
								else
									if (shapes[k].SetX > shapes[i].SetX)
									flagDown = false;
								else
									flagUp = false;
							}
						}
						if (flagUp || flagDown)
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
				{
					shape.DelX = e.X - shape.SetX;
					shape.DelY = e.Y - shape.SetY;
					if (shape.Check(e.X, e.Y))
					{
						DrawDrag = true;
						shape.DragFlag = true;						
					}
				}
				if (!DrawDrag)
				{
					if (shapes.Count > 2)
					{
						int count = 0;
						for (int i = 0; i < shapes.Count - 1; i++)
						{
							for (int j = i + 1; j < shapes.Count; j++)
							{
								bool flagUp = true;
								bool flagDown = true;
								for (int k = 0; k < shapes.Count; k++)
								{
									if (k != j && k != i && i != j)
									{
										if ((shapes[j].SetX - shapes[i].SetX) != 0)
											if ((shapes[k].SetX - shapes[i].SetX) * (shapes[j].SetY - shapes[i].SetY) / (shapes[j].SetX - shapes[i].SetX) + shapes[i].SetY <= shapes[k].SetY)
												flagUp = false;
											else
												flagDown = false;
										else
											if (shapes[k].SetX > shapes[i].SetX)
											flagDown = false;
										else
											flagUp = false;
									}
								}
								if (flagUp || flagDown)
								{
									int tempx;
									if (shapes[j].SetX < shapes[i].SetX)
										tempx = shapes[i].SetX;
									else
										tempx = shapes[j].SetX;
									/*if (shapes[j].SetY - shapes[i].SetY != 0)
									{*/
										if ((e.Y - shapes[i].SetY) * (shapes[j].SetX - shapes[i].SetX) / (shapes[j].SetY - shapes[i].SetY) + shapes[i].SetX > e.X && (e.Y - shapes[i].SetY) *(shapes[j].SetX - shapes[i].SetX) / (shapes[j].SetY - shapes[i].SetY) + shapes[i].SetX < tempx)
											count++;
									/*}
									else
										if (e.X < shapes[i].SetX && e.X < shapes[j].SetY)
										count++;*/
										//**** ***** там какая-то *****на с продолжениями прямых попробуй кароч простроить шестиугольник а дальше сама ***сь АХАХАХА ИДИ *****
								} 
							}
						}
						if (count == 1)
							DrawDragFigure = true;
					}
					if (!Draw)
						Draw = true;
					if (!DrawDragFigure)
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
			if (DrawDragFigure)
			{
				for (int i = 0; i < shapes.Count; i++)
				{
					shapes[i].SetX = e.X - shapes[i].DelX;
					shapes[i].SetY = e.Y - shapes[i].DelY;
				}
					
			}
			this.Refresh();
		}

		private void Form1_MouseUp(object sender, MouseEventArgs e)
		{
			DrawDrag = DrawDragFigure = false;
			for (int i = 0; i < shapes.Count; i++)
				shapes[i].DragFlag = false;
			if (shapes.Count > 2)
				for (int i = 0; i < shapes.Count; i++)
				{
					if (!shapes[i].IsInside)
					{
						shapes.RemoveAt(i);
						i--;
					}
				}
		}

		private void цветФигурыToolStripMenuItem_Click(object sender, EventArgs e)
		{
			colorDialog1.ShowDialog();
			Vertex.col = colorDialog1.Color;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			DoubleBuffered = true;
		}
	}
}