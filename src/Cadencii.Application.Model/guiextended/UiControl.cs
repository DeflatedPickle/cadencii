using System;
using cadencii.java.awt;
using cadencii.vsq;

namespace cadencii
{
	public interface UiControl : IDisposable
	{
		
		AnchorStyles Anchor { get; set; }

		bool TabStop { get; set; }

		Rectangle Bounds { get; set; }

		int Top { get; set; }
		int Left { get; set; }

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

		Dimension Size { get; set; }

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

		event KeyEventHandler PreviewKeyDown;
		event EventHandler<KeyPressEventArgs> KeyPress;
		event KeyEventHandler KeyUp;
		event KeyEventHandler KeyDown;
		event MouseEventHandler MouseClick;
		event MouseEventHandler MouseDoubleClick;
		event MouseEventHandler MouseDown;
		event MouseEventHandler MouseUp;
		event MouseEventHandler MouseMove;
		event MouseEventHandler MouseWheel;
		event EventHandler Enter;
		event EventHandler Resize;
		event EventHandler ImeModeChanged;

	}
	
}
