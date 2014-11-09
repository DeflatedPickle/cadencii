using System;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public interface FormInsertBar : UiForm
	{
		int Position { get; set; }
		int Length { get; set; }
	}
}

