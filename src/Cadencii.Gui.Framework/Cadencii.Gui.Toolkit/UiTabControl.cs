using System;
using System.Collections.Generic;

namespace Cadencii.Gui.Toolkit
{
	public interface UiTabControl : UiControl
	{
		int SelectedIndex { get; set; }
		bool Multiline { get; set; }
		IList<UiTabPage> Tabs { get; }
	}
}

