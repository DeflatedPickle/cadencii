using System;
using cadencii.java.awt;

namespace cadencii
{
	public interface FormIconPaletteUi : UiForm
	{
		void applyShortcut (Keys shortcut);

		bool getPreviousAlwaysOnTop ();

		void setPreviousAlwaysOnTop (bool previous);
	}
}

