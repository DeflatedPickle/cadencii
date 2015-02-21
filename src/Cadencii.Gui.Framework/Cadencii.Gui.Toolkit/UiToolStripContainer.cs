using System;
using System.Collections.Generic;
using Cadencii.Gui;

namespace Cadencii.Gui.Toolkit
{
	public interface UiToolStripContainer : UiControl
	{
		IControlContainer ContentPanel { get; }
		IControlContainer BottomToolStripPanel { get; }
	}
}

