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
