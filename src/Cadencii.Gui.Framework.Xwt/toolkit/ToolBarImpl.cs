using System;
using System.Collections.Generic;
using System.Linq;

namespace Cadencii.Gui.Toolkit
{
	public partial class ToolBarImpl : ToolBarBase, UiToolBar
	{
		event EventHandler<ToolBarButtonClickEventArgs> UiToolBar.ButtonClick {
			add {
				foreach (var b in base.Items)
					b.ButtonReleased += (o, e) => value (o, new ToolBarButtonClickEventArgs () { Button = (UiToolBarButton) b});
			}
			remove { throw new NotImplementedException (); }
		}

		// Now the app uses only Right value i.e. ignorable.
		ToolBarTextAlign UiToolBar.TextAlign { get; set; }

		// Ignore. They should be wrapped anyways.
		bool UiToolBar.Wrappable { get; set; }

		// Ignore. If Tooltip exists, they should be always shown. The app has mostly true for the value. 
		bool UiToolBar.ShowToolTips { get; set; }

		UiImageList UiToolBar.UiImageList { get; set; }

		// FIXME: ignore... cannot be supported in Xwt as is.
		bool UiToolBar.DropDownArrows { get; set; }

		// FIXME: ignore... cannot be supported in Xwt as is.
		bool UiToolBar.Divider { get; set; }

		Cadencii.Gui.Size UiToolBar.ButtonSize { get; set; }

		// ignore... Flat or Normal, doesn't matter.
		ToolBarAppearance UiToolBar.Appearance { get; set; }

		IList<UiToolBarButton> UiToolBar.Buttons {
			get { return new CastingList<UiToolBarButton, Xwt.Button> (Items, x => (UiToolBarButton) x, g => (Xwt.Button) g); }
		}

		void UiToolBar.AddButton (UiToolBarButton button)
		{
			var tb = (UiToolBar)this;
			var b = (Xwt.Button) button;
			b.WidthRequest = tb.ButtonSize.Width;
			b.HeightRequest = tb.ButtonSize.Height;
			Items.Add (b);
		}
	}
}
