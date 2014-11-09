using System;
using System.Collections.Generic;

namespace Cadencii.Gui.Toolkit
{
	public interface UiToolStripDropDownItem : UiToolStripItem
	{
		IList<UiToolStripItem> DropDownItems { get; }
		event EventHandler DropDownOpening;
	}
}

