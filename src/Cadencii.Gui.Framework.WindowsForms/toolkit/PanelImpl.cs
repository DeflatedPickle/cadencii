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
using cadencii;

namespace Cadencii.Gui.Toolkit
{
	public partial class PanelImpl : System.Windows.Forms.Panel, UiPanel
	{
		// IControlContainer

		void IControlContainer.AddControl (UiControl control)
		{
			Controls.Add ((System.Windows.Forms.Control) control);
		}

		void IControlContainer.RemoveControl (UiControl control)
		{
			Controls.Remove ((System.Windows.Forms.Control) control);
		}

		IEnumerable<UiControl> IControlContainer.GetControls ()
		{
			foreach (UiControl c in Controls)
				yield return c;
		}

		// UiPanel

		BorderStyle UiPanel.BorderStyle {
			get { return (BorderStyle)BorderStyle; }
			set { BorderStyle = (System.Windows.Forms.BorderStyle) value; }
		}
	}
}

