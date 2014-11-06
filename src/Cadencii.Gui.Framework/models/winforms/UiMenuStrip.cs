using System;
using System.Collections.Generic;
using Cadencii.Gui;

namespace cadencii
{
	public interface UiMenuStrip : UiControl
	{
		ToolStripRenderMode RenderMode { get; set; }

		string Text { get; set; }

		IList<UiToolStripItem> Items { get; }
	}
}

