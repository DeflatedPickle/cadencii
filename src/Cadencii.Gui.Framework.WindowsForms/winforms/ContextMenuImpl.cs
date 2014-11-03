using System;
using System.Linq;
using cadencii.java.awt;
using Keys = cadencii.java.awt.Keys;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using KeyEventHandler = System.Windows.Forms.KeyEventHandler;
using MouseButtons = System.Windows.Forms.MouseButtons;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using MouseEventHandler = System.Windows.Forms.MouseEventHandler;
using NKeyEventArgs = cadencii.java.awt.KeyEventArgs;
using NKeyPressEventArgs = cadencii.java.awt.KeyPressEventArgs;
using NKeyEventHandler = cadencii.java.awt.KeyEventHandler;
using NMouseButtons = cadencii.java.awt.MouseButtons;
using NMouseEventArgs = cadencii.java.awt.MouseEventArgs;
using NMouseEventHandler = cadencii.java.awt.MouseEventHandler;
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
