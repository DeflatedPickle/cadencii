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

		Size Size { get; set; }


		void Refresh ();
        
		event KeyEventHandler PreviewKeyDown;
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
