using System;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Controls
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

