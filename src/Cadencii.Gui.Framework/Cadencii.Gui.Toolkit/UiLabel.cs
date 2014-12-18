using System;

namespace Cadencii.Gui.Toolkit
{
	public interface UiLabel : UiControl, IHasText
	{
		string Text { get; set; }
	}
}

