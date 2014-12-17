using System;
using System.Collections;

namespace Cadencii.Gui.Toolkit
{
	public interface UiComboBox : UiControl
	{
		IList Items { get; }
		int SelectedIndex { get; set; }
	}
}

