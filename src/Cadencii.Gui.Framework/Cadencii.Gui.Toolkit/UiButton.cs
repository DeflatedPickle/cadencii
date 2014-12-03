using System;

namespace Cadencii.Gui.Toolkit
{
	public interface UiButton : UiControl
	{
		string Text { get; set; }
		Image Image { get; set; }
	}
}

