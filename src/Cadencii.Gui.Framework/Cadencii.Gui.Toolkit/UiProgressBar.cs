using System;

namespace Cadencii.Gui.Toolkit
{
	public interface UiProgressBar : UiControl
	{
		int Minimum { get; set; }

		int Maximum { get; set; }

		int Value { get; set; }
	}
}

