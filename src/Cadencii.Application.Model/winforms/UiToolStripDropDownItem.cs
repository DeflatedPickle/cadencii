using System;
using System.Collections.Generic;

namespace cadencii
{
	public interface UiToolStripDropDownItem : UiToolStripItem
	{
		List<UiToolStripItem> DropDownItems { get; }
		event EventHandler DropDownOpening;
	}
}

