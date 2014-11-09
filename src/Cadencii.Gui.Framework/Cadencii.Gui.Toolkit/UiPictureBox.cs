using System;
using Cadencii.Gui;

namespace Cadencii.Gui.Toolkit
{
	public interface UiPictureBox : UiControl
	{
		Cadencii.Gui.Cursor Cursor {
			get;
			set;
		}

		BorderStyle BorderStyle { get; set; }

		PictureBoxSizeMode SizeMode { get; set; }

		Dimension MaximumSize { get; set; }
		Dimension MinimumSize { get; set; }

		Image Image { get; set; }

		event EventHandler<PaintEventArgs> Paint;
	}
}

