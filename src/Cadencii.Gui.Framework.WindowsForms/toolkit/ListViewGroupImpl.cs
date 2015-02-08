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
	public class ListViewGroupImpl : UiListViewGroup
	{
		System.Windows.Forms.ListViewGroup impl;
		public ListViewGroupImpl ()
		{
			impl = new System.Windows.Forms.ListViewGroup ();
		}

		public ListViewGroupImpl (string header, HorizontalAlignment alignment)
		{
			impl = new System.Windows.Forms.ListViewGroup (header, (System.Windows.Forms.HorizontalAlignment) alignment);
		}

		public object Native {
			get { return impl; }
			set { impl = (System.Windows.Forms.ListViewGroup) value; }
		}

		public string Header {
			get { return impl.Header; }
			set { impl.Header = value; }
		}
		public string Name {
			get { return impl.Name; }
			set { impl.Name = value; }
		}

		public HorizontalAlignment HeaderAlignment {
			get { return (HorizontalAlignment)impl.HeaderAlignment; }
			set { impl.HeaderAlignment = (System.Windows.Forms.HorizontalAlignment)value; }
		}
	}
}
