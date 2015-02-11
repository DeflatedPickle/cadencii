using System;
using System.Collections.Generic;
using Cadencii.Gui;

namespace Cadencii.Gui.Toolkit
{
	public interface UiMenuStrip : UiComponent
	{
		object Native { get; }

		ToolStripRenderMode RenderMode { get; set; }

		string Text { get; set; }

		IList<UiToolStripItem> Items { get; }
	}
}

