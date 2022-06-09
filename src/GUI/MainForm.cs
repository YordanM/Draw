using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Draw
{
	/// <summary>
	/// Върху главната форма е поставен потребителски контрол,
	/// в който се осъществява визуализацията
	/// </summary>
	public partial class MainForm : Form
	{
		/// <summary>
		/// Агрегирания диалогов процесор във формата улеснява манипулацията на модела.
		/// </summary>
		private DialogProcessor dialogProcessor = new DialogProcessor();
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

		/// <summary>
		/// Изход от програмата. Затваря главната форма, а с това и програмата.
		/// </summary>
		void ExitToolStripMenuItemClick(object sender, EventArgs e)
		{
			Close();
		}
		
		/// <summary>
		/// Събитието, което се прихваща, за да се превизуализира при изменение на модела.
		/// </summary>
		void ViewPortPaint(object sender, PaintEventArgs e)
		{
			dialogProcessor.ReDraw(sender, e);
		}
		
		/// <summary>
		/// Бутон, който поставя на произволно място правоъгълник със зададените размери.
		/// Променя се лентата със състоянието и се инвалидира контрола, в който визуализираме.
		/// </summary>
		void DrawRectangleSpeedButtonClick(object sender, EventArgs e)
		{
			dialogProcessor.AddRandomRectangle();
			
			statusBar.Items[0].Text = "Последно действие: Рисуване на правоъгълник";
			
			viewPort.Invalidate();
		}

		/// <summary>
		/// Прихващане на координатите при натискането на бутон на мишката и проверка (в обратен ред) дали не е
		/// щракнато върху елемент. Ако е така то той се отбелязва като селектиран и започва процес на "влачене".
		/// Промяна на статуса и инвалидиране на контрола, в който визуализираме.
		/// Реализацията се диалогът с потребителя, при който се избира "най-горния" елемент от екрана.
		/// </summary>
		void ViewPortMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (pickUpSpeedButton.Checked) {

				Shape shapeSelected = dialogProcessor.ContainsPoint(e.Location);

                if (shapeSelected != null)
                {
                    if (dialogProcessor.Selection.Contains(shapeSelected))
                    {
						dialogProcessor.Selection.Remove(shapeSelected);
						
					}
					else
					{
						dialogProcessor.Selection.Add(shapeSelected);
					}
				}

				statusBar.Items[0].Text = "Последно действие: Селекция на примитив";
				dialogProcessor.IsDragging = true;
				dialogProcessor.LastLocation = e.Location;
				viewPort.Invalidate();
			}
		}

		/// <summary>
		/// Прихващане на преместването на мишката.
		/// Ако сме в режм на "влачене", то избрания елемент се транслира.
		/// </summary>
		void ViewPortMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (dialogProcessor.IsDragging) {
				if (dialogProcessor.Selection != null) statusBar.Items[0].Text = "Последно действие: Влачене";
				dialogProcessor.TranslateTo(e.Location);
				viewPort.Invalidate();
			}
		}

		/// <summary>
		/// Прихващане на отпускането на бутона на мишката.
		/// Излизаме от режим "влачене".
		/// </summary>
		void ViewPortMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			dialogProcessor.IsDragging = false;
		}

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
			dialogProcessor.AddRandomEllipse();

			statusBar.Items[0].Text = "Последно действие: Рисуване на елипса";

			viewPort.Invalidate();
		}

        private void openToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
			if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				dialogProcessor.ShapeList = (List<Shape>)dialogProcessor.DeSerializeFile(openFileDialog1.FileName);
				viewPort.Invalidate();
			}
			statusBar.Items[0].Text = "Последно действие: Отваряне на файл";
		}

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
			if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				dialogProcessor.SerializeFile(dialogProcessor.ShapeList, saveFileDialog1.FileName);
			}
			statusBar.Items[0].Text = "Последно действие: Записване на файл.";
		}

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (colorSelectDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (var item in dialogProcessor.Selection)
                {
					item.StrokeColor = colorSelectDialog.Color;
				}
				viewPort.Validate();
            }

			dialogProcessor.ChangeStrokeColor(colorSelectDialog.Color);

			statusBar.Items[0].Text = "Последно действие: Смяна на цвят на контура.";
		}


		public Button enterBtn;
		public Button cancelBtn;
		public TextBox widthTextBox;
		public Label text;
		public Form borderForm;
		private void toolStripButton3_Click(object sender, EventArgs e)
        {
			borderForm = new Form();
			cancelBtn = new Button();


			borderForm.Text = "Въведете дебелина на контура";
			enterBtn = new Button();
			widthTextBox = new TextBox();
			text = new Label();
			text.Text = "Въведете число между 1-20: ";
			enterBtn.Text = "Въведи дебелина на контур";
			cancelBtn.Text = "Отказ";
			text.Location = new Point(90, 80);
			widthTextBox.Location = new Point(text.Left, text.Height + text.Top + 10);
			borderForm.Controls.Add(text);
			borderForm.Controls.Add(widthTextBox);
			enterBtn.Location = new Point(widthTextBox.Left, widthTextBox.Height + widthTextBox.Top + 10);
			cancelBtn.Location = new Point(enterBtn.Left, enterBtn.Height + enterBtn.Top + 10);

			borderForm.AcceptButton = enterBtn;

			borderForm.CancelButton = cancelBtn;

			borderForm.Controls.Add(enterBtn);
			enterBtn.DialogResult = System.Windows.Forms.DialogResult.OK;

			borderForm.Controls.Add(cancelBtn);
			borderForm.StartPosition = FormStartPosition.CenterScreen;
			borderForm.ShowDialog();

			ChangeStrokeWidth(sender, e);
		}

		private void ChangeStrokeWidth(Object sender, EventArgs e)
        {
			try
			{
				if (widthTextBox.Text == "")
				{
					borderForm.Close();
				}
				else if ((float.Parse(widthTextBox.Text) < 0) || (float.Parse(widthTextBox.Text) > 20))
				{
					string message = "Въведете валидна стойност 1-20!";
					string caption = "Грешно въведени данни";
					MessageBoxButtons button = MessageBoxButtons.OK;
					DialogResult result;

					result = MessageBox.Show(message, caption, button);
					if (result == System.Windows.Forms.DialogResult.OK)
					{

					}
				}
				else
				{
					dialogProcessor.defaulStroketWidth = float.Parse(widthTextBox.Text);
					dialogProcessor.ChangeStrokeWidth(float.Parse(widthTextBox.Text));
					statusBar.Items[0].Text = "Последно действие: Задаване на дебелина на контур.";
					viewPort.Invalidate();
				}
			}
			catch
			{
				borderForm.Close();
			}
		}

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
			dialogProcessor.Rotate(10);
			statusBar.Items[0].Text = "Последно действие: Завъртане на фигура.";
			viewPort.Invalidate();
		}

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
			dialogProcessor.Rotate(15);
			statusBar.Items[0].Text = "Последно действие: Завъртане на фигура.";
			viewPort.Invalidate();
		}

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
			dialogProcessor.Rotate(25);
			statusBar.Items[0].Text = "Последно действие: Завъртане на фигура.";
			viewPort.Invalidate();
		}

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
			dialogProcessor.Rotate(50);
			statusBar.Items[0].Text = "Последно действие: Завъртане на фигура.";
			viewPort.Invalidate();
		}

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
			dialogProcessor.Rotate(75);
			statusBar.Items[0].Text = "Последно действие: Завъртане на фигура.";
			viewPort.Invalidate();
		}

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
			dialogProcessor.Rotate(100);
			statusBar.Items[0].Text = "Последно действие: Завъртане на фигура.";
			viewPort.Invalidate();
		}
    }
}
