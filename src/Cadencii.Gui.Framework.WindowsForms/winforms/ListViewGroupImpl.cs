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
