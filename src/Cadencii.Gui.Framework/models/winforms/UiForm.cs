using System;
using Cadencii.Gui;

namespace cadencii
{
	public interface UiForm : UiControl
	{
		Dimension MinimumSize { get; set; }
		bool TopMost { get; set; }

		bool InvokeRequired { get; }
		object Invoke (Delegate d, params object [] args);

		void Close ();

		event EventHandler LocationChanged;
		event EventHandler FormClosing;
		event EventHandler FormClosed;

		Cursor Cursor { get; set; }
		Point Location { get; set; }

		string Text { get; set; }

		int showDialog (object parentForm);

		void Show ();
		DialogResult ShowDialog ();
	}
}
