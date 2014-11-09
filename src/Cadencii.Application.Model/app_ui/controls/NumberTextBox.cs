using System;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public enum NumberTextBoxValueType
        {
            Double,
            Float,
            Integer,
        }

	public interface NumberTextBox : UiTextBox
	{
		NumberTextBoxValueType Type { get; set; }
	}
}

