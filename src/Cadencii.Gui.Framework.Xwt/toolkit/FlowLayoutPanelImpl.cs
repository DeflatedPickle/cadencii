using System;
using System.Collections.Generic;
using System.Linq;

namespace Cadencii.Gui.Toolkit
{
	public partial class FlowLayoutPanelImpl
	{
		IEnumerable<UiControl> UiFlowLayoutPanel.Controls {
			get { return content.Children.Cast<UiControl> (); }
		}

		Size UiFlowLayoutPanel.ClientSize {
			get { return Size.ToGui (); } // FIXME: sure?
		}

		Xwt.Box content;

		void IControlContainer.AddControl (UiControl control)
		{
			content.PackEnd ((Xwt.Widget) control);
		}

		void IControlContainer.RemoveControl (UiControl control)
		{
			content.Remove ((Xwt.Widget) control);
		}

		IEnumerable<UiControl> IControlContainer.GetControls ()
		{
			foreach (UiControl c in ((UiFlowLayoutPanel) this).Controls)
				yield return c;
		}
	}
}

