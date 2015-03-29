using System;
using System.Collections.Generic;

namespace Cadencii.Gui.Toolkit
{
	public partial class VBoxImpl : VBoxBase, UiVBox
	{
		void IControlContainer.AddControl (UiControl control)
		{
			base.PackStart ((Xwt.Widget)control);
		}

		void IControlContainer.RemoveControl (UiControl control)
		{
			base.Remove ((Xwt.Widget)control);
		}

		IEnumerable<UiControl> IControlContainer.GetControls ()
		{
			foreach (UiControl c in Children)
				yield return c;
		}

		void UiVBox.PackStart (UiControl control)
		{
			base.PackStart ((Xwt.Widget) control.Native);
		}

		void UiVBox.PackEnd (UiControl control)
		{
			base.PackEnd ((Xwt.Widget) control.Native);
		}
	}
}

