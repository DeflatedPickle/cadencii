using System;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Forms;

namespace Cadencii.Application.Controls
{
	public interface PictPianoRoll : UiPictureBox
	{
		MouseTracer mMouseTracer { get; set; }

		void setMainForm (FormMain formMain);
	}
}

