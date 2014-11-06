using System;
using Cadencii.Gui;

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

