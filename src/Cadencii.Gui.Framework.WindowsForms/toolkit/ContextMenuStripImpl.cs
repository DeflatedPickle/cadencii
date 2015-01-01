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
using System.ComponentModel;
using System.Collections.Generic;
using Cadencii.Gui.Toolkit;
using cadencii;

namespace Cadencii.Gui.Toolkit
{
	public partial class ContextMenuStripImpl : System.Windows.Forms.ContextMenuStrip, UiContextMenuStrip
	{
		public ContextMenuStripImpl (System.ComponentModel.IContainer components)
			: base (components)
		{
		}

		event EventHandler opening;
		event EventHandler UiContextMenuStrip.Opening {
			add { opening += value; }
			remove { opening -= value; }
		}

		void UiContextMenuStrip.Show (UiControl control, int x, int y)
		{
			Show ((System.Windows.Forms.Control) control, x, y);
		}

		IList<UiToolStripItem> UiContextMenuStrip.Items {
			get { return new CastingList<UiToolStripItem, System.Windows.Forms.ToolStripItem> (Items, null, null); }
		}

		ToolStripRenderMode UiContextMenuStrip.RenderMode {
			get { return (ToolStripRenderMode)RenderMode; }
			set { RenderMode = (System.Windows.Forms.ToolStripRenderMode) value; }
		}
	}
}

