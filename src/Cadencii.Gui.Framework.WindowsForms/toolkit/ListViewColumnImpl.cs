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
		int? width;
		string text;

		public ListViewColumnImpl ()
		{
		}

		public object Native {
			get { return impl; }
            set {
				impl = (System.Windows.Forms.ColumnHeader) value;
				if (width != null)
					impl.Width = (int) width;
				if (text != null)
					impl.Text = text;
			}
		}

		public int Width {
			get { return impl != null ? impl.Width : width ?? 0; }
			set {
				width = value;
				if (impl != null)
					impl.Width = value;
			}
		}

		public string Text {
			get { return impl != null ? impl.Text : text; }
			set {
				text = value;
				if (impl != null)
					impl.Text = value;
			}
		}
	}
	
}
