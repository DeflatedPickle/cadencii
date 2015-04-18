using System;

namespace Cadencii.Gui.Toolkit
{
	public interface UiSplitContainer : UiControl
	{
		IControlContainer Panel1 { get; }
		IControlContainer Panel2 { get; }
	}
	public interface UiVSplitContainer : UiSplitContainer
	{
	}
	public interface UiHSplitContainer : UiSplitContainer
	{
	}
}
