using System;
using cadencii.java.awt;

namespace cadencii
{
	public interface LyricTextBox : UiControl
	{
		void SelectAll ();

		void setPhoneticSymbolEditMode (bool b);

		void setBufferText (string phrase);

		string getBufferText ();

		string Text {
			get;
			set;
		}

		bool isPhoneticSymbolEditMode ();

		PictPianoRoll Parent {
			get;
			set;
		}

		cadencii.java.awt.Font Font {
			get;
			set;
		}

		bool Enabled {
			get;
			set;
		}

		bool Visible {
			get;
			set;
		}

		bool AcceptsReturn {
			get;
			set;
		}

		cadencii.java.awt.BorderStyle BorderStyle {
			get;
			set;
		}
	}
}

