using System;

namespace Cadencii.Gui.Toolkit
{
	public interface UiCheckBox : UiControl, IHasText
	{
		bool AutoSize { get; set; }
		// should be almost ignorable
		bool UseVisualStyleBackColor { get; set; }

		Appearance Appearance { get; set; }

		bool Checked { get; set; }
		event EventHandler Click;
		event EventHandler CheckedChanged;
	}
}

