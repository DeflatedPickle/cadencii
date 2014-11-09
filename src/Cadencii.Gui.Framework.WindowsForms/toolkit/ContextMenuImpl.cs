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
using Cadencii.Gui.Toolkit;

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

