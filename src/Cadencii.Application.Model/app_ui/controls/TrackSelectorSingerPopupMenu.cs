using System;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public interface TrackSelectorSingerPopupMenu : UiContextMenuStrip
	{
		bool SingerChangeExists { get; set; }
		int Clock { get; set; }
		int InternalID { get; set; }
		Point PointToScreen (Point source);
	}
}

