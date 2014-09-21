using System;

namespace cadencii
{
	public interface PictPianoRoll : UiControl
	{
		MouseTracer mMouseTracer { get; set; }

		cadencii.java.awt.Dimension getMinimumSize ();

		void setMainForm (object formMain);
	}
}

