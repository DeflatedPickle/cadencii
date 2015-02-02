using System;
using Cadencii.Gui;

namespace Cadencii.Gui.Toolkit
{
	public interface UiPictureBox : UiControl
	{
		BorderStyle BorderStyle { get; set; }

		PictureBoxSizeMode SizeMode { get; set; }

		Size MaximumSize { get; set; }
		Size MinimumSize { get; set; }

		Image Image { get; set; }

		event EventHandler<PaintEventArgs> Paint;

		// The same as UiForm property.
		// This is not a form specific feature, but since Control.DoubleBuffered is protected,
		// it cannot be an interface member (technically it can, but it's annoying to add impl.
		// all around) so expose it here.
		bool DoubleBuffered { get; set; }
	}
}

