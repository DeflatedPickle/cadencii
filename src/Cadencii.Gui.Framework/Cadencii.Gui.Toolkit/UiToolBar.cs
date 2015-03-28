using System;
using System.Collections.Generic;
using Cadencii.Gui;

namespace Cadencii.Gui.Toolkit
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

		UiImageList UiImageList {
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

		Size ButtonSize {
			get;
			set;
		}

		ToolBarAppearance Appearance {
			get;
			set;
		}

		IList<UiToolBarButton> Buttons { get; }
		void AddButton (UiToolBarButton button);
		event EventHandler<ToolBarButtonClickEventArgs> ButtonClick;
	}
}

