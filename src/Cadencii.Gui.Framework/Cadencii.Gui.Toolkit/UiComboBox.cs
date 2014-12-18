using System;
using System.Collections;

namespace Cadencii.Gui.Toolkit
{
	public interface UiComboBox : UiControl, IHasText
	{
		string Text { get; set; }
		IList Items { get; }
		int SelectedIndex { get; set; }
		event EventHandler SelectedIndexChanged;
		object SelectedItem { get; }
	}
}

