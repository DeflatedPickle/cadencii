using System;
using System.Collections.Generic;
using cadencii.java.awt;

namespace cadencii
{
	public interface UiMenuStrip : UiControl
	{
		ToolStripRenderMode RenderMode { get; set; }

		string Text { get; set; }

		List<UiToolStripItem> Items { get; }
	}
}

