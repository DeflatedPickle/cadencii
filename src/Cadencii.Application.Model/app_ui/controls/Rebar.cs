using System;
using Cadencii.Gui;

namespace cadencii
{
	public interface Rebar : UiControl
	{
		IntPtr RebarHwnd { get; }
		Image BackgroundImage { get; set; }
		bool ToggleDoubleClick { get; set; }
		RebarBandCollection Bands { get; }
	}
}

