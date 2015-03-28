using System;
using System.Linq;
using Cadencii.Gui;
using Keys = Cadencii.Gui.Toolkit.Keys;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using KeyEventHandler = System.Windows.Forms.KeyEventHandler;
using MouseButtons = System.Windows.Forms.MouseButtons;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using MouseEventHandler = System.Windows.Forms.MouseEventHandler;
using NKeyEventArgs = Cadencii.Gui.Toolkit.KeyEventArgs;
using NKeyPressEventArgs = Cadencii.Gui.Toolkit.KeyPressEventArgs;
using NKeyEventHandler = System.EventHandler<Cadencii.Gui.Toolkit.KeyEventArgs>;
using NMouseButtons = Cadencii.Gui.Toolkit.MouseButtons;
using NMouseEventArgs = Cadencii.Gui.Toolkit.MouseEventArgs;
using NMouseEventHandler = System.EventHandler<Cadencii.Gui.Toolkit.MouseEventArgs>;
using System.Collections.Generic;
using System.ComponentModel;
using Cadencii.Gui.Toolkit;
using cadencii;

namespace Cadencii.Gui.Toolkit
{
	public partial class ToolBarImpl : System.Windows.Forms.ToolBar, UiToolBar
	{
		UiImageList image_list;

		public ToolBarImpl ()
		{
			image_list = new ImageListImpl (this.ImageList);
		}

		event EventHandler<ToolBarButtonClickEventArgs> UiToolBar.ButtonClick {
			add { ButtonClick += (sender, e) => value (sender, ExtensionsWF.ToGui (e)); }
			remove {
				throw new NotImplementedException ();
			}
		}

		ToolBarTextAlign UiToolBar.TextAlign {
			get { return (ToolBarTextAlign) TextAlign; }
			set { TextAlign = (System.Windows.Forms.ToolBarTextAlign) value; }
		}

		public UiImageList UiImageList {
			get { return image_list; }
			set {
				image_list = value;
				this.ImageList = ((ImageListImpl) value).Native;
			}
		}

		Size UiToolBar.ButtonSize {
			get { return ButtonSize.ToGui (); }
			set { ButtonSize = value.ToWF (); }
		}

		ToolBarAppearance UiToolBar.Appearance {
			get { return (ToolBarAppearance)Appearance; }
			set { Appearance = (System.Windows.Forms.ToolBarAppearance) value; }
		}

		IList<UiToolBarButton> UiToolBar.Buttons {
			get { return new CastingList<UiToolBarButton,System.Windows.Forms.ToolBarButton> (Buttons, null, null); }
		}

		void UiToolBar.AddButton (UiToolBarButton button)
		{
			base.Buttons.Add ((ToolBarButtonImpl) button);
		}
	}
}

