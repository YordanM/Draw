using System;
using System.Drawing;

namespace Draw
{
	/// <summary>
	/// Класът правоъгълник е основен примитив, който е наследник на базовия Shape.
	/// </summary>
	[Serializable]
	public class EllipseShape : Shape
	{
		#region Constructor

		public EllipseShape(RectangleF rect) : base(rect)
		{
		}

		public EllipseShape(RectangleShape rectangle) : base(rectangle)
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

		public bool ContainsCheck(PointF location)
		{

			int a = (int)(Rectangle.Left + (Rectangle.Width / 2));
			int b = (int)(Rectangle.Top + (Rectangle.Height / 2));
			PointF center = new Point(
				a, b);

			double _xRadius = Rectangle.Width / 2;
			double _yRadius = Rectangle.Height / 2;


			if (_xRadius <= 0.0 || _yRadius <= 0.0)
				return false;

			PointF normalized = new PointF(location.X - center.X,
										 location.Y - center.Y);

			return ((double)(normalized.X * normalized.X)
					 / (_xRadius * _xRadius)) + ((double)(normalized.Y * normalized.Y) / (_yRadius * _yRadius))
				<= 1.0;
		}

		public override bool Contains(PointF point)
		{
			if (ContainsCheck(point))
				// Проверка дали е в обекта само, ако точката е в обхващащия правоъгълник.
				// В случая на правоъгълник - директно връщаме true
				return true;
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
			base.Rotate(grfx);

			grfx.FillEllipse(new SolidBrush(FillColor), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
			grfx.DrawEllipse(new Pen(StrokeColor, StrokeWidth), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);

			grfx.ResetTransform();
		}
	}
}
