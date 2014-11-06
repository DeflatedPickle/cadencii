using System;
using Cadencii.Gui;

namespace cadencii
{
	public interface LyricTextBox : UiTextBox
	{
		void setPhoneticSymbolEditMode (bool b);

		void setBufferText (string phrase);

		string getBufferText ();

		bool isPhoneticSymbolEditMode ();

		PictPianoRoll Parent {
			get;
			set;
		}
	}
}

