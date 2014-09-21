using System;
using cadencii.java.awt;

namespace cadencii
{
	public interface FormIconPaletteUi : UiBase
	{
		void applyShortcut (Keys shortcut);

		bool getPreviousAlwaysOnTop ();

		void setPreviousAlwaysOnTop (bool previous);
	}
}

