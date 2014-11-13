using System;
using Cadencii.Gui.Toolkit;

namespace Cadencii.Application.Forms
{
	public interface FormInsertBar : UiForm
	{
		int Position { get; set; }
		int Length { get; set; }
	}
}

