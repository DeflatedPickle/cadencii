using System;
using System.Collections.Generic;

namespace Cadencii.Gui.Toolkit
{
	public interface UiFlowLayoutPanel : UiControl, IControlContainer
	{
		IEnumerable<UiControl> Controls { get; }
		Size ClientSize { get; }
	}
}

