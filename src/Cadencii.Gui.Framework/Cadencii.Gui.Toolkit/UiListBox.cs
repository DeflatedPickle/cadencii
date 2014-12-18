using System;
using System.Collections;

namespace Cadencii.Gui.Toolkit
{
	public interface UiListBox : UiControl
	{
		int ItemHeight { get; set; }
		int SelectedIndex { get; set; }
		IList SelectedIndices { get; }
		bool FormattingEnabled { get; set; }
		IList Items { get; }
		event EventHandler SelectedIndexChanged;
	}
}

