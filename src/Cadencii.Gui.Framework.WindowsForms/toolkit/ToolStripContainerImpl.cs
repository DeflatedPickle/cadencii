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
	public partial class ToolStripContainerImpl : System.Windows.Forms.ToolStripContainer, UiToolStripContainer
	{
		class PanelWrapper : IControlContainer
		{
			System.Windows.Forms.Control panel;

			public PanelWrapper (System.Windows.Forms.Control panel)
			{
				this.panel = panel;
			}
			
			#region IControlContainer implementation
			void IControlContainer.AddControl (UiControl control)
			{
				panel.Controls.Add ((System.Windows.Forms.Control) control);
			}
			void IControlContainer.RemoveControl (UiControl control)
			{
				panel.Controls.Remove ((System.Windows.Forms.Control) control);
			}
			#endregion
		}

		IControlContainer UiToolStripContainer.ContentPanel {
			get { return new PanelWrapper (ContentPanel); }
		}
		IControlContainer UiToolStripContainer.BottomToolStripPanel {
			get { return new PanelWrapper (BottomToolStripPanel); }
		}
	}
}

