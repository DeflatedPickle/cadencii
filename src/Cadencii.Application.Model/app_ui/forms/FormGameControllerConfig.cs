using System;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public interface FormGameControllerConfig : UiForm
	{
			
		#region public methods

		void applyLanguage ();

		int getRectangle ();

		int getTriangle ();

		int getCircle ();

		int getCross ();

		int getL1 ();

		int getL2 ();

		int getR1 ();

		int getR2 ();

		int getSelect ();

		int getStart ();

		int getPovDown ();

		int getPovLeft ();

		int getPovUp ();

		int getPovRight ();

		#endregion
	}
}

