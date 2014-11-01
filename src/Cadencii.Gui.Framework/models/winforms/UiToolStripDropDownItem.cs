using System;
using System.Collections.Generic;

namespace cadencii
{
	public interface UiToolStripDropDownItem : UiToolStripItem
	{
		IList<UiToolStripItem> DropDownItems { get; }
		event EventHandler DropDownOpening;
	}
}

