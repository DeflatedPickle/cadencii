using System;
using System.Linq;
using Cadencii.Gui;
using Keys = Cadencii.Gui.Keys;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using KeyEventHandler = System.Windows.Forms.KeyEventHandler;
using MouseButtons = System.Windows.Forms.MouseButtons;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using MouseEventHandler = System.Windows.Forms.MouseEventHandler;
using NKeyEventArgs = Cadencii.Gui.KeyEventArgs;
using NKeyPressEventArgs = Cadencii.Gui.KeyPressEventArgs;
using NKeyEventHandler = Cadencii.Gui.KeyEventHandler;
using NMouseButtons = Cadencii.Gui.MouseButtons;
using NMouseEventArgs = Cadencii.Gui.MouseEventArgs;
using NMouseEventHandler = Cadencii.Gui.MouseEventHandler;
using System.Collections.Generic;

namespace cadencii
{
	public class ContextMenuImpl : System.Windows.Forms.ContextMenu, UiContextMenu
	{
		#region UiContextMenu implementation
		void UiContextMenu.Show (UiControl control, Point point)
		{
			Show ((System.Windows.Forms.Control) control, point.ToWF ());
		}

		IList<UiMenuItem> UiContextMenu.MenuItems {
			get { return new CastingList<UiMenuItem, System.Windows.Forms.MenuItem> (MenuItems, null, null); }
		}
		#endregion
	}
}

