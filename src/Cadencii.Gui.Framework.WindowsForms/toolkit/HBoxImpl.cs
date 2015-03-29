using System;

namespace Cadencii.Gui.Toolkit
{
	public class HBoxImpl : FlowLayoutPanelImpl, UiHBox
	{
		public HBoxImpl ()
		{
			base.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
		}

		void UiHBox.PackStart (UiControl control)
		{
			var c = (System.Windows.Forms.Control)control;
			base.Controls.Add (c);
			base.Controls.SetChildIndex (c, 0);
		}

		void UiHBox.PackEnd (UiControl control)
		{
			base.Controls.Add ((System.Windows.Forms.Control)control);
		}
	}
}

