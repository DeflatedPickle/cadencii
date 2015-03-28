using System;
using System.Collections.Generic;

namespace Cadencii.Gui.Toolkit
{
	public class ContextMenuImpl : ContextMenuBase, UiContextMenu
	{
		List<UiMenuItem> menuitems = new List<UiMenuItem> ();

		#region UiContextMenu implementation

		void UiContextMenu.Show (UiControl control, Point point)
		{
			throw new NotImplementedException ();
		}

		System.Collections.Generic.IList<UiMenuItem> UiContextMenu.MenuItems {
			get { return menuitems; }
		}

		#endregion
	}
}

