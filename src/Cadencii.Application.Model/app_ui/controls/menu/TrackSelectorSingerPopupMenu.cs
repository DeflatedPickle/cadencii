using System;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Controls
{
	public interface TrackSelectorSingerPopupMenu : UiContextMenuStrip
	{
		bool SingerChangeExists { get; set; }
		int Clock { get; set; }
		int InternalID { get; set; }
	}
}

