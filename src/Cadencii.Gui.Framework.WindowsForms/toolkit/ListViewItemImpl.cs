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

	public class ListViewItemImpl : UiListViewItem
	{
		System.Windows.Forms.ListViewItem impl;

		public ListViewItemImpl (System.Windows.Forms.ListViewItem impl)
		{
			this.impl = impl;
		}

		UiListViewItem UiListViewItem.GetSubItem (int i)
		{
			return new ListViewItemImpl (impl);
		}

		public object Native {
			get { return impl; }
		}

		string UiListViewItem.Text {
			get { return impl.Text; }
		}

		bool UiListViewItem.Checked {
			get { return impl.Checked; }
			set { impl.Checked = value; }
		}
	}
	
}
