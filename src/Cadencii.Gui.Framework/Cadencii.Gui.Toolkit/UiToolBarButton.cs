using System;
using Cadencii.Gui;

namespace Cadencii.Gui.Toolkit
{
	public interface UiToolBarButton : UiComponent
	{
		string Name {
			get;
			set;
		}

		bool Enabled {
			get;
			set;
		}

		bool Pushed { get; set; }
		string ImageKey { get; set; }
		int ImageIndex { get; set; }
		object Tag { get; set; }
		string Text { get; set; }
		string ToolTipText { get; set; }
		Rectangle Rectangle { get; }
	}
}

