using System;
using cadencii.java.awt;

namespace cadencii
{
	public interface UiToolBarButton : UiComponent
	{
		bool Pushed { get; set; }
		string ImageKey { get; set; }
		int ImageIndex { get; set; }
		ToolBarButtonStyle Style { get; set; }
		object Tag { get; set; }
		string Text { get; set; }
		string ToolTipText { get; set; }
		Rectangle Rectangle { get; set; }
	}
}

