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
using System.ComponentModel;
using Cadencii.Gui.Toolkit;

namespace cadencii
{
	public class ToolTipImpl : System.Windows.Forms.ToolTip, UiToolTip
	{
		event EventHandler<DrawToolTipEventArgs> UiToolTip.Draw {
			add { Draw += (sender, e) => value (sender, e.ToAwt ()); }
			remove {
				throw new NotImplementedException ();
			}
		}

		public ToolTipImpl (IContainer components)
			: base (components)
		{
		}

		#region UiToolTip implementation
		void UiToolTip.Show (string text, UiControl control, Point point, int duration)
		{
			Show (text, (System.Windows.Forms.Control)control, point.ToWF (), duration);
		}

		void UiToolTip.Hide (UiControl control)
		{
			Hide ((System.Windows.Forms.Control)control.Native);
		}
		#endregion
	}
}