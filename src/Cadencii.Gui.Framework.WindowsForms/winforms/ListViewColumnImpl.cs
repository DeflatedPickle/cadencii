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

	public class ListViewColumnImpl : UiListViewColumn
	{
		System.Windows.Forms.ColumnHeader impl;

		public ListViewColumnImpl (System.Windows.Forms.ColumnHeader impl)
		{
			this.impl = impl;
		}

		public object Native {
			get { return impl; }
		}

		int UiListViewColumn.Width {
			get { return impl.Width; }
			set { impl.Width = value; }
		}
	}
	
}
