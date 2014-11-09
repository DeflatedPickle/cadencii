using System;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public interface FormRandomize : UiForm
	{
		int getResolution ();

		int getStartBar ();

		int getStartBeat ();

		int getEndBar ();

		int getEndBeat ();

		bool isPositionRandomizeEnabled ();

		int getPositionRandomizeValue ();

		bool isPitRandomizeEnabled ();

		int getPitRandomizeValue ();

		int getPitRandomizePattern ();

		void applyLanguage ();
	}
}

