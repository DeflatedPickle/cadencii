using System;
using cadencii.java.awt;

namespace cadencii
{
	public interface FormIconPaletteUi : UiBase
	{
		Point Location { get; set; }

		void applyShortcut (Keys shortcut);

		bool getPreviousAlwaysOnTop ();

		void setPreviousAlwaysOnTop (bool previous);

		event EventHandler LocationChanged;
	}
}

