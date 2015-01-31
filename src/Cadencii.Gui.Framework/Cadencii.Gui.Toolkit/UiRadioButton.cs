using System;

namespace Cadencii.Gui.Toolkit
{
	public interface UiRadioButton : UiControl, IHasText
	{
		bool Checked { get; set; }
		event EventHandler CheckedChanged;
	}
}

