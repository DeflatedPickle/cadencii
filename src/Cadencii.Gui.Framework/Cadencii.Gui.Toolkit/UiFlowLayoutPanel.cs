using System;

namespace Cadencii.Gui.Toolkit
{
	public interface UiFlowLayoutPanel : UiControl, IControlContainer
	{
		Size ClientSize { get; }
	}
}

