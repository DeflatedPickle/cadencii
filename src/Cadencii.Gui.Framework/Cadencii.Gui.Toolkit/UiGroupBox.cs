using System;

namespace Cadencii.Gui.Toolkit
{
	public interface UiGroupBox : UiControl, IControlContainer
	{
		string Text { get; set; }
	}
}

