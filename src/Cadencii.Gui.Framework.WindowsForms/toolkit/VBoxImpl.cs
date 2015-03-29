using System;

namespace Cadencii.Gui.Toolkit
{
	public class VBoxImpl : FlowLayoutPanelImpl, UiVBox
	{
		public VBoxImpl ()
		{
			base.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
		}

		void UiVBox.PackStart (UiControl control)
		{
			var c = (System.Windows.Forms.Control)control;
			base.Controls.Add (c);
			base.Controls.SetChildIndex (c, 0);
		}

		void UiVBox.PackEnd (UiControl control)
		{
			base.Controls.Add ((System.Windows.Forms.Control)control);
		}
	}
}

