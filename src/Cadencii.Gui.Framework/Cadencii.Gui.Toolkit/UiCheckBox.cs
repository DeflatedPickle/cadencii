using System;

namespace Cadencii.Gui.Toolkit
{
	public interface UiCheckBox : UiControl
	{
		string Text { get; set; }
		bool Checked { get; set; }
		event EventHandler CheckedChanged;
	}
}

