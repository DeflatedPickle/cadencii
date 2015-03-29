using System;
using System.Collections.Generic;

namespace Cadencii.Gui.Toolkit
{
	public partial class HBoxImpl : HBoxBase, UiHBox
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

		void UiHBox.PackStart (UiControl control)
		{
			base.PackStart ((Xwt.Widget) control.Native);
		}

		void UiHBox.PackEnd (UiControl control)
		{
			base.PackEnd ((Xwt.Widget) control.Native);
		}
	}
}

