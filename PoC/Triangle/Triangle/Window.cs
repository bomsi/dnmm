using System;
using System.Windows.Forms;

namespace Triangle
{
	public partial class Window : Form
	{
		private readonly Action _formClosedAction;
		private readonly Action _paintAction;

		public Window(Action formClosedAction, Action paintAction)
		{
			_formClosedAction = formClosedAction;
			_paintAction = paintAction;

			InitializeComponent();
		}

		private void Window_FormClosed(object sender, FormClosedEventArgs e)
		{
			_formClosedAction();
		}

		private void Window_Paint(object sender, PaintEventArgs e)
		{
			_paintAction();
		}
	}
}
