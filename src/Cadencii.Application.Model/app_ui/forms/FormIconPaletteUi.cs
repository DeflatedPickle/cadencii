using System;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public interface FormIconPaletteUi : UiForm
	{
		void applyShortcut (Keys shortcut);

		bool getPreviousAlwaysOnTop ();

		void setPreviousAlwaysOnTop (bool previous);
	}
}

