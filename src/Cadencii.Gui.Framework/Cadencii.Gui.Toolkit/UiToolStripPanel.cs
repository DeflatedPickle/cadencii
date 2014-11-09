using System;
using Cadencii.Gui;
using System.Collections.Generic;

namespace Cadencii.Gui.Toolkit
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

