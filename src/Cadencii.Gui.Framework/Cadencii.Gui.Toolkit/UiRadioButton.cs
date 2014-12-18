using System;

namespace Cadencii.Gui.Toolkit
{
	public interface UiRadioButton : UiControl, IHasText
	{
		string Text { get; set; }
		bool Checked { get; set; }
		event EventHandler CheckedChanged;
	}
}

