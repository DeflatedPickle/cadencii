using System;
using Cadencii.Gui;

namespace Cadencii.Gui.Toolkit
{
	public interface UiPanel : UiControl, IControlContainer
	{
		bool AutoScroll { get; set; }
		BorderStyle BorderStyle { get; set; }
	}
}

