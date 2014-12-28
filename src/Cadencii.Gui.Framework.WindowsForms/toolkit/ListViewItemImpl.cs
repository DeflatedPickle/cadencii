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

	public class ListViewItemImpl : UiListViewItem
	{
		System.Windows.Forms.ListViewItem impl;

		public ListViewItemImpl (System.Windows.Forms.ListViewItem impl)
		{
			this.impl = impl;
		}

		bool UiListViewItem.Selected {
			get { return impl.Selected; }
			set { impl.Selected = value; }
		}

		UiListViewItem UiListViewItem.GetSubItem (int i)
		{
			return new ListViewItemImpl (impl);
		}

		public object Native {
			get { return impl; }
		}

		Color UiListViewItem.BackColor {
			get { return impl.BackColor.ToGui (); }
			set { impl.BackColor = value.ToNative (); }
		}

		string UiListViewItem.Text {
			get { return impl.Text; }
			set { impl.Text = value; }
		}

		bool UiListViewItem.Checked {
			get { return impl.Checked; }
			set { impl.Checked = value; }
		}
	}
	
}
