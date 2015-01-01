using System;

namespace Cadencii.Gui.Toolkit
{
	public interface UiButton : UiControl, IHasText
	{
		Image Image { get; set; }
	}
}

