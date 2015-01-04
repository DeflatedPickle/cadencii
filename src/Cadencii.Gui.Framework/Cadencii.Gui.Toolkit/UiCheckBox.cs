using System;

namespace Cadencii.Gui.Toolkit
{
	public interface UiCheckBox : UiControl, IHasText
	{
		bool Checked { get; set; }
		event EventHandler CheckedChanged;
	}
}

