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
	public class ListViewGroupImpl : UiListViewGroup
	{
		System.Windows.Forms.ListViewGroup impl;
		public ListViewGroupImpl (string header, HorizontalAlignment alignment)
		{
			impl = new System.Windows.Forms.ListViewGroup (header, (System.Windows.Forms.HorizontalAlignment) alignment);
		}

		public object Native {
			get { return impl; }
		}

		public string Header {
			get { return impl.Header; }
			set { impl.Header = value; }
		}
		public string Name {
			get { return impl.Name; }
			set { impl.Name = value; }
		}
	}
	
}
