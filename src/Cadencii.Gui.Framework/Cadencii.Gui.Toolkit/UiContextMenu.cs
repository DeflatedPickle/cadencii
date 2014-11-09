using System;
using Cadencii.Gui;
using System.Collections.Generic;

namespace Cadencii.Gui.Toolkit
{
	public interface UiContextMenu : UiComponent
	{
		IList<UiMenuItem> MenuItems { get; }

		void Show (UiControl control, Point point);
	}
}

