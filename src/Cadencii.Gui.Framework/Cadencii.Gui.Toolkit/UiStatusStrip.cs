using System;
using System.Collections.Generic;

namespace Cadencii.Gui.Toolkit
{
	public interface UiStatusStrip : UiControl
	{
		string Text {
			get;
			set;
		}

		IList<UiToolStripItem> Items {
			get;
		}
	}
}

