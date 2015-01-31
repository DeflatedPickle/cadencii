using System;
using System.Collections.Generic;

namespace Cadencii.Gui.Toolkit
{
	public partial class ToolBarImpl : ToolBarBase, UiToolBar
	{
		event EventHandler<ToolBarButtonClickEventArgs> UiToolBar.ButtonClick {
			add { base.ButtonPressed += (o, e) => value (o, e.ToGui ()); }
			remove { throw new NotImplementedException (); }
		}

		// Now the app uses only Right value i.e. ignorable.
		ToolBarTextAlign UiToolBar.TextAlign { get; set; }

		// Ignore. They should be wrapped anyways.
		bool UiToolBar.Wrappable { get; set; }

		// Ignore. If Tooltip exists, they should be always shown. The app has mostly true for the value. 
		bool UiToolBar.ShowToolTips { get; set; }

		UiImageList UiToolBar.UiImageList {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		// FIXME: ignore... cannot be supported in Xwt as is.
		bool UiToolBar.DropDownArrows { get; set; }

		// FIXME: ignore... cannot be supported in Xwt as is.
		bool UiToolBar.Divider { get; set; }

		Cadencii.Gui.Size UiToolBar.ButtonSize {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		// ignore... Flat or Normal, doesn't matter.
		ToolBarAppearance UiToolBar.Appearance { get; set; }

		IEnumerable<UiToolBarButton> UiToolBar.Buttons {
			get {
				throw new NotImplementedException ();
			}
		}

		void UiToolBar.AddButton (UiToolBarButton button)
		{
			throw new NotImplementedException ();
		}
	}
}
