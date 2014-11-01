using System;
using cadencii.java.awt;

namespace cadencii
{
	public interface UiForm : UiControl
	{
		bool TopMost { get; set; }

		object Invoke (Delegate d);

		void Close ();

		event EventHandler LocationChanged;
		event EventHandler FormClosing;
		event EventHandler FormClosed;

		Point Location { get; set; }

		string Text { get; set; }

		int showDialog (object parentForm);

		void Show ();
		DialogResult ShowDialog ();
	}
}
