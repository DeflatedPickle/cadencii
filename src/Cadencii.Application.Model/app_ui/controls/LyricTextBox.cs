using System;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Controls
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

