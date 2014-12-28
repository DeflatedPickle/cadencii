using System;
using Cadencii.Gui;
using System.ComponentModel;

namespace Cadencii.Gui.Toolkit
{
	public interface UiForm : UiControl
	{
		FormBorderStyle FormBorderStyle { get; set; }
		Size ClientSize { get; set; }
		double Opacity { get; set; }
		DialogResult DialogResult { get; set; }

		IContainer Components { get; }

		// This is not a form specific feature, but since Control.DoubleBuffered is protected,
		// it cannot be an interface member (technically it can, but it's annoying to add impl.
		// all around) so expose it here.
		bool DoubleBuffered { get; set; }

		// custom property that accesses "Styles"
		bool UserPaint { get; set; }
		bool AllPaintingInWmPaint { get; set; }

		UiForm AsGui ();

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

		Size MinimumSize { get; set; }
		bool TopMost { get; set; }

		bool InvokeRequired { get; }
		object Invoke (Delegate d, params object [] args);

		void Close ();

		event EventHandler LocationChanged;
		event EventHandler<FormClosingEventArgs> FormClosing;
		event EventHandler FormClosed;

		string Text { get; set; }

		void Show ();
		DialogResult ShowDialog ();
		DialogResult ShowDialog (object parentForm);
	}
}
