using System;

namespace Cadencii.Gui.Toolkit
{
	public interface UiLabel : UiControl, IHasText
	{
		ContentAlignment TextAlign { get; set; }
		string Text { get; set; }
	}
}

