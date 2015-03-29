using System;

namespace Cadencii.Gui.Toolkit
{
	public partial class VBoxImpl : VBoxBase, UiVBox
	{
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

