using System;
using cadencii.java.awt;

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

