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
using System.Windows.Forms;
using System.ComponentModel;

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
			Show (text, (Control)control, point.ToWF (), duration);
		}

		void UiToolTip.Hide (UiControl control)
		{
			Hide ((Control)control.Native);
		}
		#endregion
	}
}