using System;
using System.Collections.Generic;

namespace Cadencii.Gui.Toolkit
{
	public class HBoxImpl : FlowLayoutPanelImpl, UiHBox, IControlContainer
	{
		public HBoxImpl ()
		{
			base.AutoScroll = true;
			base.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			base.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
		}

		void IControlContainer.AddControl (UiControl control)
		{
			base.Controls.Add ((System.Windows.Forms.Control)control);
		}

		void IControlContainer.RemoveControl (UiControl control)
		{
			base.Controls.Remove ((System.Windows.Forms.Control)control);
		}

		IEnumerable<UiControl> IControlContainer.GetControls ()
		{
			foreach (UiControl c in Controls)
				yield return c;
		}

		void UiHBox.PackStart (UiControl control)
		{
			var c = (System.Windows.Forms.Control)control;
			base.Controls.Add (c);
		}

		void UiHBox.PackEnd (UiControl control)
		{
			throw new NotImplementedException ();
		}
	}
}

