using System;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Controls
{
	public interface Rebar : UiControl
	{
		// usable only within winforms implementation. Do not expect to be usable outside the impl.
		IntPtr RebarHwnd { get; }
		bool ToggleDoubleClick { get; set; }
		RebarBandCollection Bands { get; }
	}
}

