using System;
using cadencii.java.awt;
using cadencii.vsq;

namespace cadencii
{
	public interface UiUserControl : UiControl
	{
		BorderStyle BorderStyle { get; set; }
	}

	public interface UiControl
	{
		
		cadencii.java.awt.AnchorStyles Anchor {
			get;
			set;
		}

		bool TabStop {
			get;
			set;
		}

		Rectangle Bounds { get; set; }

		Point PointToClient (Point point);
		Point PointToScreen (Point point);

		event EventHandler Resize;

		event EventHandler ImeModeChanged;

		int Top { get; set; }
		int Left { get; set; }

		void SuspendLayout ();
		void ResumeLayout ();

		void Focus ();

		ImeMode ImeMode { get; set; }

		bool IsDisposed { get; }

		object Native { get; }
		Padding Margin { get; set; }
		string Name { get; set; }
		int TabIndex { get; set; }
		DockStyle Dock { get; set; }
		int Width { get; set; }
		int Height { get; set; }
		void Dispose ();

		Color BackColor { get; set; }

		Point Location { get; set; }

		Dimension Size { get; set; }


		void Refresh ();
        
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
	}
	
}
