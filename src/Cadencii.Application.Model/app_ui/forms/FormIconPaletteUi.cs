using System;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Forms
{
	public interface FormIconPaletteUi : UiForm
	{
		void applyShortcut (Keys shortcut);

		bool getPreviousAlwaysOnTop ();

		void setPreviousAlwaysOnTop (bool previous);
	}
}

