using System;
using Cadencii.Gui;
using System.Collections.Generic;

namespace cadencii
{
	public interface UiToolStripPanel : UiControl
	{
		List<UiControl> Controls {
			get;
		}

		ToolStripRenderMode RenderMode {
			get;
			set;
		}
	}
}

