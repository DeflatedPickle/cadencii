using System;
using System.Collections.Generic;

namespace cadencii
{
	public interface UiStatusStrip : UiControl
	{
		string Text {
			get;
			set;
		}

		List<UiToolStripItem> Items {
			get;
		}
	}
}

