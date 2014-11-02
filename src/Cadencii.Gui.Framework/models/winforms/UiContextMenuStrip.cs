using System;
using System.Collections.Generic;
using cadencii.java.awt;

namespace cadencii
{
	public interface UiContextMenuStrip : UiControl
	{
		bool ShowImageMargin { get; set; }
		bool ShowCheckMargin { get; set; }
		IList<UiToolStripItem> Items { get; }
		ToolStripRenderMode RenderMode { get; set; }
		void Show (UiControl control, int x, int y);
		event EventHandler Opening;
	}
}

