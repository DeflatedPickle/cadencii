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
	public partial class SplitContainerImpl : System.Windows.Forms.SplitContainer, UiSplitContainer
	{
		IControlContainer panel1, panel2;

		public SplitContainerImpl ()
		{
			panel1 = new PanelWrapper (this.Panel1);
			panel2 = new PanelWrapper (this.Panel2);
		}

		IControlContainer UiSplitContainer.Panel1 {
			get { return panel1; }
		}

		IControlContainer UiSplitContainer.Panel2 {
			get { return panel2; }
		}
	}

	public partial class HSplitContainerImpl : SplitContainerImpl, UiHSplitContainer
	{
		public HSplitContainerImpl ()
		{
			Orientation = System.Windows.Forms.Orientation.Horizontal;
		}
	}

	public partial class VSplitContainerImpl : SplitContainerImpl, UiVSplitContainer
	{
		public VSplitContainerImpl ()
		{
			Orientation = System.Windows.Forms.Orientation.Vertical;
		}
	}
}

