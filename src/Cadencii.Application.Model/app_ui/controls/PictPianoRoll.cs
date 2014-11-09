using System;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public interface PictPianoRoll : UiPictureBox
	{
		MouseTracer mMouseTracer { get; set; }

		Cadencii.Gui.Dimension getMinimumSize ();

		void setMainForm (FormMain formMain);
	}
}

