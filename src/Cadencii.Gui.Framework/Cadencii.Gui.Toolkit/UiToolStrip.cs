using System;
using System.Collections.Generic;
using Cadencii.Gui;

namespace Cadencii.Gui.Toolkit
{
	public interface UiToolStrip : UiControl
	{
		ToolStripRenderMode RenderMode { get; set; }

		IList<UiToolStripItem> Items { get; }
	}
}

