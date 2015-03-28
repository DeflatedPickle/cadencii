using System;

namespace Cadencii.Gui.Toolkit
{
	public interface UiCheckBox : UiControl, IHasText
	{
		// should be almost ignorable
		bool UseVisualStyleBackColor { get; set; }

		bool Checked { get; set; }
		event EventHandler Click;
		event EventHandler CheckedChanged;
	}
}

