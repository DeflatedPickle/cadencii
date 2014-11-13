using System;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Forms
{
	public interface FormDeleteBar : UiForm
	{
		int Start { get; set; }
		int End { get; set; }
	}
}

