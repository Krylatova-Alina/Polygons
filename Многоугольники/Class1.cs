using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Многоугольники
{  
    public abstract class Vertex
    {
        protected int x0;
        protected int y0;
        public static int R
        {
            get; set;
        }
        public static Color col
        {
            get; set;
        }
        public int SetX { set { x0 = value; } get { return x0; } }
        public int SetY { set { y0 = value; } get { return y0; } }
        public Vertex()
        {
        }
        public Vertex(int x, int y)
        {            
            this.x0 = x;
            this.y0 = y;
        }
        static Vertex()
        {
            R = 40;
            col = Color.Black;
        }
        abstract public void DrawFigure(Graphics graph);
        abstract public bool Check(int x, int y);
    }

    class Circle : Vertex
    {
        public Circle(int x, int y) : base(x, y)
        {
        }
        public override void DrawFigure(Graphics graph)
        {
            SolidBrush br = new SolidBrush(col);
            graph.FillEllipse(br, x0 - R / 2, y0 - R / 2, R, R);
        }
        public override bool Check(int x, int y) 
        {
            if ((x - x0) * (x-x0) + (y-y0) * (y-y0) <= R * R /4)
                return true;
            else
                return false;
        }
    }
    class Rectangle : Vertex
    {
        public Rectangle(int x, int y) : base(x, y)
        {
        }
        public override void DrawFigure(Graphics graph)
        {
            SolidBrush br = new SolidBrush(col);
            graph.FillRectangle(br, x0 - R/2, y0 - R/2, R, R);
        }
        public override bool Check(int x, int y)
        {
            if (Math.Abs(x-x0) <= R/2 && Math.Abs(y - y0) <= R/2)
                return true;
            else
                return false;
        }
    }
    class Triangle : Vertex
    {
        public Triangle(int x, int y) : base(x, y)
        {
        }
        public override void DrawFigure(Graphics graph)
        {
            SolidBrush br = new SolidBrush(col);
            Point point1 = new Point(x0 - R / 2, y0 + R / 2);
            Point point2 = new Point(x0, y0 - R / 2);
            Point point3 = new Point(x0 + R / 2, y0 + R / 2);
            Point[] curvePoints = { point1, point2, point3 };
            graph.FillPolygon(br, curvePoints);
        }
        public override bool Check(int x, int y)
        {
            if (y - y0 + R / 2 >= 2 * x0 - 2 * x && y - y0 + R / 2 >= 2 * x - 2 * x0 && y <= y0 + R / 2)

                return true;
            else
                return false;
        }
    }
}
