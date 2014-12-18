using System;

namespace Cadencii.Gui.Toolkit
{
	public interface UiButton : UiControl, IHasText
	{
		string Text { get; set; }
		Image Image { get; set; }
	}
}

