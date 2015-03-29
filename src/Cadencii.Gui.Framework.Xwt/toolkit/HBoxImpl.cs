using System;

namespace Cadencii.Gui.Toolkit
{
	public partial class HBoxImpl : HBoxBase, UiHBox
	{
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

