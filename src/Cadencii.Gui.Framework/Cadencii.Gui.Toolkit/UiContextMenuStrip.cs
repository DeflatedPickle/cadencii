using System;
using System.Collections.Generic;
using Cadencii.Gui;

namespace Cadencii.Gui.Toolkit
{
	public interface UiContextMenuStrip : UiControl
	{
		// FIXME: should be removed, cannot be implemented
		bool ShowImageMargin { get; set; }
		// FIXME: should be removed, cannot be implemented
		bool ShowCheckMargin { get; set; }
		IList<UiToolStripItem> Items { get; }
		// FIXME: should be removed, cannot be implemented
		ToolStripRenderMode RenderMode { get; set; }
		void Show (UiControl control, int x, int y);
		// FIXME: should be removed, cannot be implemented
		event EventHandler Opening;
		// FIXME: should be removed, cannot be implemented
		event EventHandler VisibleChanged;
	}
}

