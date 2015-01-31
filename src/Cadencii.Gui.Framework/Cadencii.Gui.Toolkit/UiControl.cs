using System;
using Cadencii.Gui;
using System.Collections.Generic;

namespace Cadencii.Gui.Toolkit
{
	public interface UiControl : IDisposable
	{
		bool Focused { get; }

		UiControl Parent { get; }

		void Invoke (Delegate e);

		void BringToFront ();

		Cursor Cursor { get; set; }

		void PerformLayout ();

		AnchorStyles Anchor { get; set; }

		bool TabStop { get; set; }

		Rectangle Bounds { get; set; }

		int Top { get; set; }
		int Left { get; set; }
		int Bottom { get; }
		int Right { get; }

		ImeMode ImeMode { get; set; }

		bool IsDisposed { get; }

		object Native { get; }
		Padding Margin { get; set; }
		string Name { get; set; }
		int TabIndex { get; set; }
		DockStyle Dock { get; set; }
		int Width { get; set; }
		int Height { get; set; }

		Color BackColor { get; set; }
		Color ForeColor { get; set; }

		Point Location { get; set; }

		Size Size { get; set; }

		bool Enabled { get; set; }

		bool Visible { get; set; }

		Font Font { get; set; }

		Point PointToClient (Point point);
		Point PointToScreen (Point point);

		void SuspendLayout ();
		void ResumeLayout ();
		void ResumeLayout (bool performLayout);

		void Focus ();

		void Refresh ();
        
		void Invalidate ();
		void Invalidate (bool invalidateChildren);

		event EventHandler<KeyEventArgs> PreviewKeyDown;
		event EventHandler<KeyEventArgs> KeyUp;
		event EventHandler<KeyEventArgs> KeyDown;
		event EventHandler<MouseEventArgs> MouseClick;
		event EventHandler<MouseEventArgs> MouseDoubleClick;
		event EventHandler<MouseEventArgs> MouseDown;
		event EventHandler<MouseEventArgs> MouseUp;
		event EventHandler<MouseEventArgs> MouseMove;
		event EventHandler<MouseEventArgs> MouseWheel;
		event EventHandler Enter;
		event EventHandler Leave;
		event EventHandler SizeChanged;
		event EventHandler Resize;
	}
	
}
