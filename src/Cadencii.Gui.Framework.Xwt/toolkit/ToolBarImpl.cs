using System;
using System.Collections.Generic;
using System.Linq;

namespace Cadencii.Gui.Toolkit
{
	public partial class ToolBarImpl : ToolBarBase, UiToolBar
	{
		class ToolBarButtonImpl : UiToolBarButton
		{

			Xwt.Button b;

			public ToolBarButtonImpl (Xwt.Button b)
			{
				this.b = b;
			}

			#region UiToolBarButton implementation

			string UiToolBarButton.Name {
				get { return b.Name; }
				set { b.Name = value; }
			}

			bool UiToolBarButton.Enabled {
				get { return b.Sensitive; }
				set { b.Sensitive = value; }
			}

			bool UiToolBarButton.Pushed {
				get { return ((Xwt.ToggleButton)b).Active; }
				set { ((Xwt.ToggleButton)b).Active = value; }
			}

			// cannot support
			string UiToolBarButton.ImageKey { get; set; }

			// cannot support
			int UiToolBarButton.ImageIndex { get; set; }

			object UiToolBarButton.Tag {
				get { return b.Tag; }
				set { b.Tag = value; }
			}

			string UiToolBarButton.Text {
				get { return b.Label; }
				set { b.Label = value; }
			}

			string UiToolBarButton.ToolTipText {
				get { return b.TooltipText; }
				set { b.TooltipText = value; }
			}

			Rectangle UiToolBarButton.Rectangle {
				get { return b.ScreenBounds.ToGui (); }
			}

			#endregion
		}

		event EventHandler<ToolBarButtonClickEventArgs> UiToolBar.ButtonClick {
			add {
				foreach (var b in base.Items)
					b.ButtonReleased += (o, e) => value (o, new ToolBarButtonClickEventArgs () { Button = new ToolBarButtonImpl (b) });
			}
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

		Cadencii.Gui.Size UiToolBar.ButtonSize { get; set; }

		// ignore... Flat or Normal, doesn't matter.
		ToolBarAppearance UiToolBar.Appearance { get; set; }

		IEnumerable<UiToolBarButton> UiToolBar.Buttons {
			get { return Items.Cast<UiToolBarButton> (); }
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
