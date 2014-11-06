using System;
using Cadencii.Gui;

namespace cadencii
{
	public interface UiPanel : UiControl
	{
		BorderStyle BorderStyle { get; set; }
	}
}

