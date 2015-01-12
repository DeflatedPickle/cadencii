using System;

namespace Cadencii.Gui.Toolkit
{
	public interface UiButton : UiControl, IHasText
	{
		event EventHandler Click;
		Image Image { get; set; }
	}
}

