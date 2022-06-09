using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Draw.src.Model
{
	[Serializable]
	public class EclipseShape : Shape
	{
		#region Constructor
		public EclipseShape()
        {

        }
		public EclipseShape(RectangleF rect) : base(rect)
		{
			
		}

		public EclipseShape(RectangleShape rectangle) : base(rectangle)
		{
		}
		float x, y, h, w;
		float radius;
		int radius2;
		int x2, y2;
		public void DrawEclipse()
        {

        }

		#endregion

		/// <summary>
		/// Проверка за принадлежност на точка point към правоъгълника.
		/// В случая на правоъгълник този метод може да не бъде пренаписван, защото
		/// Реализацията съвпада с тази на абстрактния клас Shape, който проверява
		/// дали точката е в обхващащия правоъгълник на елемента (а той съвпада с
		/// елемента в този случай).
		/// </summary>
		/// 
		public bool containsCheck(PointF point)
        {
			//radius= sqrt(Plosht/PI)
				radius = (float)Math.Sqrt((Rectangle.Width* Rectangle.Height) / Math.PI);
			//radius = (Rectangle.Width+Rectangle.Height) / 4;
			//	x = (Rectangle.Right +Rectangle.Left)/2 - point.X;
			//	y = (Rectangle.Top + Rectangle.Bottom)/2 - point.Y;
			y = point.Y -(Rectangle.Top + Rectangle.Bottom) / 2 ;
			x = point.X-(Rectangle.Right + Rectangle.Left) / 2 ;
			x2 = Convert.ToInt32(x);
			y2 = Convert.ToInt32(y);
			radius2 = Convert.ToInt32(radius);
			int ras=(x2* x2) +(y2 * y2);
			int rad=(radius2* radius2);
			if (ras <= rad) {
				return true;
			}
            else
            {
				return false;
            }
		}

		public bool Containss( PointF location)
		{

			int a = (int)(Rectangle.Left + (Rectangle.Width / 2));
			int b = (int)(Rectangle.Top + (Rectangle.Height / 2));
			PointF center = new Point(
				a,b);

			double _xRadius = Rectangle.Width / 2;
			double _yRadius = Rectangle.Height / 2;


			if (_xRadius <= 0.0 || _yRadius <= 0.0)
				return false;
			/* This is a more general form of the circle equation
             *
             * X^2/a^2 + Y^2/b^2 <= 1
             */

			PointF normalized = new PointF(location.X - center.X,
										 location.Y - center.Y);

			return ((double)(normalized.X * normalized.X)
					 / (_xRadius * _xRadius)) + ((double)(normalized.Y * normalized.Y) / (_yRadius * _yRadius))
				<= 1.0;
		}
		public override bool Contains(PointF point)
		{
			if (Containss(point))
			{
				// Проверка дали е в обекта само, ако точката е в обхващащия правоъгълник.
				// В случая на правоъгълник - директно връщаме true

				return true;
			}
			else
				// Ако не е в обхващащия правоъгълник, то неможе да е в обекта и => false
				return false;
				
			
		}

		/// <summary>
		/// Частта, визуализираща конкретния примитив.
		/// </summary>
		public override void DrawSelf(Graphics grfx)
		{
			base.DrawSelf(grfx);
			if (matrix != null)
			{
				grfx.MultiplyTransform(matrix);
			}

			grfx.FillEllipse(new SolidBrush(FillColor), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
			grfx.DrawEllipse(new Pen(BorderColor), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);

		}
	}
}
