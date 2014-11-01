using System;

namespace cadencii
{
	public interface PictPianoRoll : UiPictureBox
	{
		MouseTracer mMouseTracer { get; set; }

		cadencii.java.awt.Dimension getMinimumSize ();

		void setMainForm (UiFormMain formMain);
	}
}

