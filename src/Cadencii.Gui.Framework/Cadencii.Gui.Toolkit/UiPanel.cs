using System;
using Cadencii.Gui;

namespace Cadencii.Gui.Toolkit
{
	public interface UiPanel : UiControl, IControlContainer
	{
		BorderStyle BorderStyle { get; set; }
	}
}

