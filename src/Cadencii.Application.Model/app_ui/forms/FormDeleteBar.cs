using System;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public interface FormDeleteBar : UiForm
	{
		int Start { get; set; }
		int End { get; set; }
	}
}

