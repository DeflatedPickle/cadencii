using System;

namespace Cadencii.Gui.Toolkit
{
	public interface UiCheckBox : UiControl, IHasText
	{
		string Text { get; set; }
		bool Checked { get; set; }
		event EventHandler CheckedChanged;
	}
}

