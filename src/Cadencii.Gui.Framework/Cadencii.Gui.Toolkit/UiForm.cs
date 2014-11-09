using System;
using Cadencii.Gui;

namespace Cadencii.Gui.Toolkit
{
	public interface UiForm : UiControl
	{
		UiForm AsAwt ();

		AutoScaleMode AutoScaleMode { get ;set ;}
		UiMenuStrip MainMenuStrip { get; set; }

		FormWindowState WindowState { get; set; }
		event EventHandler Load;
		event EventHandler Activated;
		event EventHandler Deactivate;
		event EventHandler<DragEventArgs> DragEnter;
		event EventHandler<DragEventArgs> DragDrop;
		event EventHandler<DragEventArgs> DragOver;
		event EventHandler DragLeave;

		Dimension MinimumSize { get; set; }
		bool TopMost { get; set; }

		bool InvokeRequired { get; }
		object Invoke (Delegate d, params object [] args);

		void Close ();

		event EventHandler LocationChanged;
		event EventHandler<FormClosingEventArgs> FormClosing;
		event EventHandler FormClosed;

		Cursor Cursor { get; set; }
		Point Location { get; set; }

		string Text { get; set; }

		int showDialog (object parentForm);

		void Show ();
		DialogResult ShowDialog ();
	}
}
