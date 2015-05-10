using System;

namespace Cadencii.Gui.Toolkit
{
	public interface UiLabel : UiControl, IHasText
	{
		bool AutoSize { get; set; }
		ContentAlignment TextAlign { get; set; }
		string Text { get; set; }
	}
}

