using System;

namespace Cadencii.Gui.Toolkit
{
	public interface UiTabPage : UiControl, IHasText
	{
		bool AutoScroll { get; set; }
		Padding Padding { get; set; }
	}
}

