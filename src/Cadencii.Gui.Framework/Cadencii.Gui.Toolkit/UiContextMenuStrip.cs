using System;
using System.Collections.Generic;
using Cadencii.Gui;

namespace Cadencii.Gui.Toolkit
{
	public interface UiContextMenuStrip : UiControl
	{
		bool ShowImageMargin { get; set; }
		bool ShowCheckMargin { get; set; }
		IList<UiToolStripItem> Items { get; }
		ToolStripRenderMode RenderMode { get; set; }
		void Show (UiControl control, int x, int y);
		event EventHandler Opening;
		event EventHandler VisibleChanged;
	}
}

