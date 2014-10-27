using System;
using System.Collections.Generic;
using cadencii.java.awt;

namespace cadencii
{
	// ok... there are MainForm and FormMain. Confusing at all.
	public interface MainFormUi : UiForm
	{
		bool isMouseMiddleButtonDowned (MouseButtons mouseButtons);

		int calculateStartToDrawX ();

		IList<ValuePairOfStringArrayOfKeys> getDefaultShortcutKeys ();

		void refreshScreen ();

		void updateScriptShortcut ();

		void setEdited (bool b);

		void updateDrawObjectList ();

	}
}

