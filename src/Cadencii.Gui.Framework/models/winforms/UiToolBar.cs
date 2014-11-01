using System;
using System.Collections.Generic;
using cadencii.java.awt;

namespace cadencii
{
	public interface UiToolBar : UiControl
	{
		ToolBarTextAlign TextAlign {
			get;
			set;
		}

		bool Wrappable {
			get;
			set;
		}

		bool ShowToolTips {
			get;
			set;
		}

		UiImageList ImageList {
			get;
			set;
		}

		bool DropDownArrows {
			get;
			set;
		}

		bool Divider {
			get;
			set;
		}

		Dimension ButtonSize {
			get;
			set;
		}

		ToolBarAppearance Appearance {
			get;
			set;
		}

		List<UiToolBarButton> Buttons { get; }
		event EventHandler<UiToolBarButtonEventArgs> ButtonClick;
	}
}

