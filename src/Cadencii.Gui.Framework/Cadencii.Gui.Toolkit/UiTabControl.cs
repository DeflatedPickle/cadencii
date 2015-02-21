using System;
using System.Collections.Generic;

namespace Cadencii.Gui.Toolkit
{
	public interface UiTabControl : UiControl
	{
		IList<UiTabPage> Tabs { get; }
	}
}

