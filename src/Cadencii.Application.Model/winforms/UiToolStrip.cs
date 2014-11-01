using System;
using System.Collections.Generic;
using cadencii.java.awt;

namespace cadencii
{
	public interface UiToolStrip : UiControl
	{
		ToolStripRenderMode RenderMode { get; set; }

		List<UiToolStripItem> Items { get; }
	}
}

