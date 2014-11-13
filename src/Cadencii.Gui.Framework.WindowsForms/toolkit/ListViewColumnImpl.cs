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

namespace Cadencii.Gui.Toolkit
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
