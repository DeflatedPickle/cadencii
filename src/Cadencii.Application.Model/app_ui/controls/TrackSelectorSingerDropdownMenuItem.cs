using System;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public interface TrackSelectorSingerDropdownMenuItem : UiToolStripMenuItem
	{
		int ToolTipPxWidth { get; set; }
		string ToolTipText { get; set; }
		int Language { get; set; }
		int Program { get; set; }
	}
}

