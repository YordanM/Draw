using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Draw
{
	/// <summary>
	/// Класът, който ще бъде използван при управляване на диалога.
	/// </summary>
	public class DialogProcessor : DisplayProcessor
	{
		#region Constructor

		public DialogProcessor()
		{
		}

		#endregion

		#region Properties

		public float defaulStroketWidth = 1;
		public Color defaultStrokeColor = Color.Black;
		public int defaultAngle = 0;

		/// <summary>
		/// Избран елемент.
		/// </summary>
		private List<Shape> selection = new List<Shape>();
		public List<Shape> Selection
		{
            get { return selection; }
			set { selection = value; }
		}

		/// <summary>
		/// Дали в момента диалога е в състояние на "влачене" на избрания елемент.
		/// </summary>
		private bool isDragging;
		public bool IsDragging
		{
			get { return isDragging; }
			set { isDragging = value; }
		}

		/// <summary>
		/// Последна позиция на мишката при "влачене".
		/// Използва се за определяне на вектора на транслация.
		/// </summary>
		private PointF lastLocation;
		public PointF LastLocation
		{
			get { return lastLocation; }
			set { lastLocation = value; }
		}

		#endregion

		/// <summary>
		/// Добавя примитив - правоъгълник на произволно място върху клиентската област.
		/// </summary>
		public void AddRandomRectangle()
		{
			Random rnd = new Random();
			int x = rnd.Next(100, 1000);
			int y = rnd.Next(100, 600);

			RectangleShape rect = new RectangleShape(new Rectangle(x, y, 100, 200));
			rect.FillColor = Color.White;
			rect.StrokeWidth = defaulStroketWidth;
			rect.StrokeColor = defaultStrokeColor;

			ShapeList.Add(rect);
		}

		public void AddRandomEllipse()
		{
			Random rnd = new Random();
			int x = rnd.Next(100, 1000);
			int y = rnd.Next(100, 600);

			EllipseShape ellipse = new EllipseShape(new Rectangle(x, y, 100, 200));
            ellipse.FillColor = Color.White;
            ellipse.StrokeWidth = defaulStroketWidth;
            ellipse.StrokeColor = defaultStrokeColor;

            ShapeList.Add(ellipse);
		}

		/// <summary>
		/// Проверява дали дадена точка е в елемента.
		/// Обхожда в ред обратен на визуализацията с цел намиране на
		/// "най-горния" елемент т.е. този който виждаме под мишката.
		/// </summary>
		/// <param name="point">Указана точка</param>
		/// <returns>Елемента на изображението, на който принадлежи дадената точка.</returns>
		public Shape ContainsPoint(PointF point)
		{
			for (int i = ShapeList.Count - 1; i >= 0; i--)
			{
				if (ShapeList[i].Contains(point))
				{
					ShapeList[i].FillColor = Color.Red;

					return ShapeList[i];
				}
			}
			return null;
		}

		/// <summary>
		/// Транслация на избраният елемент на вектор определен от <paramref name="p>p</paramref>
		/// </summary>
		/// <param name="p">Вектор на транслация.</param>
		public void TranslateTo(PointF p)
		{
			if (Selection != null)
			{
				foreach (var item in Selection)
				{
					item.Location = new PointF(item.Location.X + p.X - lastLocation.X, item.Location.Y + p.Y - lastLocation.Y);
				}

				lastLocation = p;
			}
		}

		public object DeSerializeFile(string path = null)
		{
			object currentObject;

			Stream stream;
			IFormatter formatter = new BinaryFormatter();

			if (path == null)
			{
				stream = new FileStream("DrawFile.asd", FileMode.Open);

			}
			else
			{
				stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
			}

			currentObject = formatter.Deserialize(stream);
			stream.Close();
			return currentObject;
		}

		public void SerializeFile(object currentObject, string path = null)
		{

			Stream stream;
			IFormatter binaryFormatter = new BinaryFormatter();

			if (path == null)
			{
				stream = new FileStream("WinFormFile.asd", FileMode.Create, FileAccess.Write, FileShare.None);
			}
			else
			{
				string preparePath = path + ".asd";
				stream = new FileStream(preparePath, FileMode.Create);
			}

			binaryFormatter.Serialize(stream, currentObject);
			stream.Close();
		}

		public void ChangeStrokeColor(Color color)
        {
			if (Selection != null)
			{
				foreach (var item in Selection)
				{
					item.StrokeColor = color;
				}
			}
        }

		public void ChangeStrokeWidth(float width)
        {
			if (Selection != null)
			{
				foreach (var item in Selection)
				{
					item.StrokeWidth = width;
				}
			}
		}

		public void Rotate(int angle)
        {
			if (Selection != null)
			{
				foreach (var item in Selection)
				{
					item.Angle = angle;
				}
			}
		}
	}
}
