using System;

namespace Cadencii.Gui.Toolkit
{
	public interface UiButton : UiControl, IHasText
	{
		event EventHandler Click;
		bool UseVisualStyleBackColor { get; set; }
		Image Image { get; set; }
	}
}

