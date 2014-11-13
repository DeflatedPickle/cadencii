using System;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Controls
{
	public interface Rebar : UiControl
	{
		IntPtr RebarHwnd { get; }
		Image BackgroundImage { get; set; }
		bool ToggleDoubleClick { get; set; }
		RebarBandCollection Bands { get; }
	}
}

