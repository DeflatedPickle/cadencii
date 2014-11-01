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

		IList<UiToolStripItem> Items {
			get;
		}
	}
}

