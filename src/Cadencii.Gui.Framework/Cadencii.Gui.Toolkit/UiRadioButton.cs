using System;

namespace Cadencii.Gui.Toolkit
{
	public interface UiRadioButton : UiControl
	{
		bool Checked { get; set; }
		event EventHandler CheckedChanged;
	}
}

