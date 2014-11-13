using System;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Forms;

namespace Cadencii.Application.Controls
{
	public interface PictPianoRoll : UiPictureBox
	{
		MouseTracer mMouseTracer { get; set; }

		Cadencii.Gui.Dimension getMinimumSize ();

		void setMainForm (FormMain formMain);
	}
}

